using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;

namespace MultithreadTwainDemo
{
	/// <summary>
	/// This demo shows how to run DotTwain in a separate thread.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private ScanControl _scanner;

		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuFile;
		private System.Windows.Forms.MenuItem menuSelectSource;
		private System.Windows.Forms.MenuItem menuAcquire;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuHideInterface;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuExit;
		private Atalasoft.Imaging.WinControls.ThumbnailView thumbnailView1;
        private Atalasoft.Imaging.WinControls.WorkspaceViewer workspaceViewer1;
        private MenuItem menuItem1;
        private MenuItem menuAbout;
        private IContainer components;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}

			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuSelectSource = new System.Windows.Forms.MenuItem();
            this.menuAcquire = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuHideInterface = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuAbout = new System.Windows.Forms.MenuItem();
            this.thumbnailView1 = new Atalasoft.Imaging.WinControls.ThumbnailView();
            this.workspaceViewer1 = new Atalasoft.Imaging.WinControls.WorkspaceViewer();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuItem1});
            // 
            // menuFile
            // 
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuSelectSource,
            this.menuAcquire,
            this.menuItem4,
            this.menuHideInterface,
            this.menuItem6,
            this.menuExit});
            this.menuFile.Text = "&File";
            // 
            // menuSelectSource
            // 
            this.menuSelectSource.Index = 0;
            this.menuSelectSource.Text = "&Select Source...";
            this.menuSelectSource.Click += new System.EventHandler(this.menuSelectSource_Click);
            // 
            // menuAcquire
            // 
            this.menuAcquire.Index = 1;
            this.menuAcquire.Text = "&Acquire";
            this.menuAcquire.Click += new System.EventHandler(this.menuAcquire_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // menuHideInterface
            // 
            this.menuHideInterface.Index = 3;
            this.menuHideInterface.Text = "&Hide Driver Interface";
            this.menuHideInterface.Click += new System.EventHandler(this.menuHideInterface_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 4;
            this.menuItem6.Text = "-";
            // 
            // menuExit
            // 
            this.menuExit.Index = 5;
            this.menuExit.Text = "E&xit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAbout});
            this.menuItem1.Text = "&Help";
            // 
            // menuAbout
            // 
            this.menuAbout.Index = 0;
            this.menuAbout.Text = "&About ...";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // thumbnailView1
            // 
            this.thumbnailView1.BackColor = System.Drawing.SystemColors.Window;
            this.thumbnailView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.thumbnailView1.DragSelectionColor = System.Drawing.Color.Red;
            this.thumbnailView1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.thumbnailView1.HighlightBackgroundColor = System.Drawing.SystemColors.Highlight;
            this.thumbnailView1.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            this.thumbnailView1.LoadErrorMessage = "";
            this.thumbnailView1.Location = new System.Drawing.Point(0, 0);
            this.thumbnailView1.Margins = new Atalasoft.Imaging.WinControls.Margin(4, 4, 4, 4);
            this.thumbnailView1.SelectionMode = Atalasoft.Imaging.WinControls.ThumbnailSelectionMode.SingleSelect;
            this.thumbnailView1.Name = "thumbnailView1";
            this.thumbnailView1.SelectionRectangleBackColor = System.Drawing.Color.Transparent;
            this.thumbnailView1.SelectionRectangleDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.thumbnailView1.SelectionRectangleLineColor = System.Drawing.Color.Black;
            this.thumbnailView1.Size = new System.Drawing.Size(160, 430);
            this.thumbnailView1.TabIndex = 0;
            this.thumbnailView1.Text = "thumbnailView1";
            this.thumbnailView1.ThumbnailBackground = null;
            this.thumbnailView1.ThumbnailOffset = new System.Drawing.Point(0, 0);
            this.thumbnailView1.ThumbnailSize = new System.Drawing.Size(100, 100);
            this.thumbnailView1.SelectedIndexChanged += new System.EventHandler(this.thumbnailView1_SelectedIndexChanged);
            // 
            // workspaceViewer1
            // 
            this.workspaceViewer1.AntialiasDisplay = Atalasoft.Imaging.WinControls.AntialiasDisplayMode.ScaleToGray;
            this.workspaceViewer1.DisplayProfile = null;
            this.workspaceViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workspaceViewer1.Location = new System.Drawing.Point(160, 0);
            this.workspaceViewer1.Magnifier.BackColor = System.Drawing.Color.White;
            this.workspaceViewer1.Magnifier.BorderColor = System.Drawing.Color.Black;
            this.workspaceViewer1.Magnifier.Size = new System.Drawing.Size(100, 100);
            this.workspaceViewer1.Name = "workspaceViewer1";
            this.workspaceViewer1.OutputProfile = null;
            this.workspaceViewer1.Selection = null;
            this.workspaceViewer1.Size = new System.Drawing.Size(496, 430);
            this.workspaceViewer1.TabIndex = 1;
            this.workspaceViewer1.Text = "workspaceViewer1";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(656, 430);
            this.Controls.Add(this.workspaceViewer1);
            this.Controls.Add(this.thumbnailView1);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Multithread DotTwain Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			this._scanner = new ScanControl(this);
			this._scanner.ImageAcquired += new Atalasoft.Twain.ImageAcquiredEventHandler(_scanner_ImageAcquired);
			this._scanner.AcquireCanceled += new EventHandler(_scanner_AcquireCanceled);
			this._scanner.AcquireFinished += new EventHandler(_scanner_AcquireFinished);
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (this._scanner != null)
				this._scanner.Dispose();
		}

		private void menuSelectSource_Click(object sender, System.EventArgs e)
		{
			this._scanner.ShowSelectSource();
		}

		private void menuAcquire_Click(object sender, System.EventArgs e)
		{
			this.menuSelectSource.Enabled = false;
			this.menuAcquire.Enabled = false;

			this._scanner.StartScan(this.menuHideInterface.Checked);
		}

		private void menuHideInterface_Click(object sender, System.EventArgs e)
		{
			this.menuHideInterface.Checked = !this.menuHideInterface.Checked;
		}

		private void menuExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void thumbnailView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// Load the selected image into the WorkspaceViewer.
			if (this.thumbnailView1.FocusedItem != null)
				this.workspaceViewer1.Image = this.workspaceViewer1.Images[this.thumbnailView1.SelectedIndices[0]];
			else
				this.workspaceViewer1.Image = null;
		}

		#region Scan Events

		private void _scanner_ImageAcquired(object sender, Atalasoft.Twain.AcquireEventArgs e)
		{
			// Convert the .NET Bitmap to an AtalaImage and dispose the Bitmap.
			Atalasoft.Imaging.AtalaImage image = Atalasoft.Imaging.AtalaImage.FromBitmap(e.Image);
			e.Image.Dispose();

			// For this demo we keep the images in memory.
			// If several images are being scanned it's better to 
			// save the to a folder and load them as needed.
			this.workspaceViewer1.Images.Add(image);
			this.thumbnailView1.Items.Add(image, "");
		}

		private void _scanner_AcquireCanceled(object sender, EventArgs e)
		{
			// The user canceled the scan.
			this.menuSelectSource.Enabled = true;
			this.menuAcquire.Enabled = true;
		}

		private void _scanner_AcquireFinished(object sender, EventArgs e)
		{
			// The scan is finished.
			this.menuSelectSource.Enabled = true;
			this.menuAcquire.Enabled = true;
		}

		#endregion

        private void menuAbout_Click(object sender, EventArgs e)
        {
            AtalaDemos.AboutBox.About aboutBox = new AtalaDemos.AboutBox.About("About Atalasoft Multithread TWAIN Demo", "Multithread TWAIN Demo");
            aboutBox.Description = @"A no-frills version of our Acquisition Demo which shows how to place the scanning control into a separate thread. Unlike the Acquisition demo, this requires either DotImage Document Imaging or a combination of DotTwain and DotImage PhotoPro licenses to run, as it makes use of our ThumbnailViewer and WorkspaceViewer controls.";
            aboutBox.ShowDialog();
        }

	}
}
