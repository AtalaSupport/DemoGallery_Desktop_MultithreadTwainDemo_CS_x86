using System;
using System.Threading;

namespace MultithreadTwainDemo
{
	/// <summary>
	/// This class controls the scanning thread and DotTwain objects.
	/// </summary>
	public class ScanControl : IDisposable
	{
		private Thread _scanningThread;
		private bool _isDisposed;
		private bool _scanning;
		private bool _hideInterface;
		private ScanControlAction _action;
		private Atalasoft.Twain.Acquisition _acquisition;
		private Atalasoft.Twain.Device _device;
		private System.Windows.Forms.Form _uiForm;

		// This variable is used to notify the scanning thread
		// when it needs to perform an action.
		private object _scanControlMessage;

		public event EventHandler AcquireCanceled;
		public event EventHandler AcquireFinished;
		public event Atalasoft.Twain.ImageAcquiredEventHandler ImageAcquired;
		
		private System.Windows.Forms.MethodInvoker _onAcquireFinished;
		private System.Windows.Forms.MethodInvoker _onAcquireCanceled;
		private Atalasoft.Twain.ImageAcquiredEventHandler _onImageAcquired;

		public ScanControl(System.Windows.Forms.Form uiForm)
		{
			// The main UI form is used for invoking.
			this._uiForm = uiForm;

			this._onImageAcquired = new Atalasoft.Twain.ImageAcquiredEventHandler(OnImageAcquired);
			this._onAcquireCanceled = new System.Windows.Forms.MethodInvoker(OnAcquireCanceled);
			this._onAcquireFinished = new System.Windows.Forms.MethodInvoker(OnAcquireFinished);
			
			this._scanControlMessage = new object();

			this._scanningThread = new Thread(new ThreadStart(RunScanningThread));
			this._scanningThread.Name = "Scanning Thread";
			this._scanningThread.Start();
		}

		~ScanControl()
		{
			Dispose();
		}

		#region Methods and Properties

		/// <summary>
		/// Stops the scanning process and disposes any resources.
		/// </summary>
		public void Dispose()
		{
			if (this._isDisposed) return;

			lock (this._scanControlMessage)
			{
				this._action = ScanControlAction.Dispose;
				Monitor.Pulse(this._scanControlMessage);
			}

			this._isDisposed = true;
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Gets a value indicating if the ScanControl has been disposed.
		/// </summary>
		public bool IsDisposed
		{
			get { return this._isDisposed; }
		}

		/// <summary>
		/// Gets a value indicating whether or not we are currently scanning.
		/// </summary>
		public bool Scanning
		{
			get { return this._scanning; }
		}

		/// <summary>
		/// Displays the 'Select Source' dialog so the user can select the device.
		/// </summary>
		public void ShowSelectSource()
		{
			lock (this._scanControlMessage)
			{
				this._action = ScanControlAction.SelectSource;
				Monitor.Pulse(this._scanControlMessage);
			}
		}

		/// <summary>
		/// Starts the scan process.
		/// </summary>
		/// <param name="hideInterface">If True, the driver interface will be hidden.</param>
		public void StartScan(bool hideInterface)
		{
			this._hideInterface = hideInterface;

			lock (this._scanControlMessage)
			{
				this._action = ScanControlAction.Start;
				Monitor.Pulse(this._scanControlMessage);
			}
		}

		#endregion

		/// <summary>
		/// This method is run from the scanning thread.
		/// The Acquisition and Device objects are created
		/// used and destroyed in this thread.
		/// </summary>
		private void RunScanningThread()
		{
			_acquisition = new Atalasoft.Twain.Acquisition();

			// If the system does not have TWAIN installed or devices, end.
			if (!_acquisition.SystemHasTwain || _acquisition.Devices.Count == 0)
			{
				_acquisition.Dispose();
				_acquisition = null;
				return;
			}

			_device = _acquisition.Devices.Default;

			// Setup the TWAIN events.
			_acquisition.ImageAcquired += new Atalasoft.Twain.ImageAcquiredEventHandler(_acquisition_ImageAcquired);
			_acquisition.AcquireCanceled += new EventHandler(_acquisition_AcquireCanceled);
			_acquisition.AcquireFinished += new EventHandler(_acquisition_AcquireFinished);

			// We loop until the control has been disposed.
			do
			{
				try
				{
					lock (this._scanControlMessage)
					{
						// Wait for the Scan Controller to tell us to do something.
						Monitor.Wait(_scanControlMessage);
					}

					// If we are told to dispose, clean up and exit the thread.
					if (_action == ScanControlAction.Dispose)
					{
						DisposeObjects();
						break;
					}

					if (_action == ScanControlAction.SelectSource)
					{
						if (_acquisition != null)
						{
							Atalasoft.Twain.Device dev = _acquisition.ShowSelectSource();
							if (dev != null)
							{
								// Ignore if it's the same device.
								if (_device != dev)
								{
									if (_device != null) _device.Close();
									_device = dev;
									_scanning = false;
								}
							}
						}
					}
					else if (_action == ScanControlAction.Start)
					{
						// Start a new scanning session.
						// We must perform a modal acquire here to
						// prevent the thread from going into a Wait.
						if (_device != null)
						{
							_scanning = true;
							_device.HideInterface = this._hideInterface;
							_device.ModalAcquire = true;
							_device.Acquire();
						}
					}
				}
				catch (ThreadAbortException)
				{
					// Abort was called on the Thread.
					DisposeObjects();
					break;
				}

			} while (this._isDisposed == false);

		}

		private void DisposeObjects()
		{
			if (_device != null)
			{
				// Calling Close will force any current scan to stop.
				_device.Close();
				_device = null;
			}

			if (this._acquisition != null)
			{
				_acquisition.Dispose();
				_acquisition = null;
			}
						
			_scanning = false;
		}

		#region TWAIN Events

		private void _acquisition_ImageAcquired(object sender, Atalasoft.Twain.AcquireEventArgs e)
		{
			// If the form is closed, force a cancel.
			if (this._isDisposed)
			{
				e.Image.Dispose();
				e.CancelPending = true;
				return;
			}

			this._uiForm.Invoke(_onImageAcquired, new object[] { null, e });
		}

		private void OnImageAcquired(object sender, Atalasoft.Twain.AcquireEventArgs e)
		{
			if (this.ImageAcquired != null)
				this.ImageAcquired(this, e);
		}

		private void _acquisition_AcquireCanceled(object sender, EventArgs e)
		{
			if (this._isDisposed) return;
			this._scanning = false;
			this._uiForm.Invoke(_onAcquireCanceled);
		}

		private void OnAcquireCanceled()
		{
			if (this.AcquireCanceled != null)
				this.AcquireCanceled(this, EventArgs.Empty);
		}

		private void _acquisition_AcquireFinished(object sender, EventArgs e)
		{
			if (this._isDisposed) return;
			this._scanning = false;
			this._uiForm.Invoke(_onAcquireFinished);
		}

		private void OnAcquireFinished()
		{
			if (this.AcquireFinished != null)
				this.AcquireFinished(this, EventArgs.Empty);
		}

		#endregion

	}

	public enum ScanControlAction
	{
		Start,
		Dispose,
		SelectSource
	}
}
