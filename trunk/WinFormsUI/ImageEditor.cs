using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

using Dataweb.Utilities;
using Dataweb.Diagramming.Advanced;


namespace Dataweb.Diagramming.WinFormsUI {

	public partial class ImageEditor : Form {

		public ImageEditor() {
			InitializeComponent();
		}


		public ImageEditor(string fileName) {
			InitializeComponent();
			if (fileName == null) throw new ArgumentNullException("fileName");
			resultImage.Load(fileName);
		}


		public ImageEditor(Image image) {
			InitializeComponent();
			if (image == null) throw new ArgumentNullException("image");
			resultImage.Image = (Image)image.Clone();
			resultImage.Name = string.Empty;
		}


		public ImageEditor(Image image, string name) {
			InitializeComponent();
			if (image == null) throw new ArgumentNullException("image");
			if (name == null) throw new ArgumentNullException("name");
			resultImage.Image = (Image)image.Clone();
			resultImage.Name = name;
		}


		public ImageEditor(NamedImage namedImage) {
			InitializeComponent();
			if (namedImage == null) throw new ArgumentNullException("namedImage");
			if (namedImage.Image != null)
				resultImage.Image = (Image)namedImage.Image.Clone();
			resultImage.Name = namedImage.Name;
		}


		public NamedImage Result { get { return resultImage; } }
		
		
		private Image Image {
			get { return resultImage.Image; }
			set {
				resultImage.Image = value;
				DisplayResult();
			}
		}

		
		private void okButton_Click(object sender, EventArgs e) {
			if (Modal) DialogResult = DialogResult.OK;
			else Close();
		}

		
		private void cancelButton_Click(object sender, EventArgs e) {
			if (Modal) DialogResult = DialogResult.Cancel;
			else Close();
		}

		
		private void clearButton_Click(object sender, EventArgs e) {
			this.SuspendLayout();
			resultImage.Image = null;
			resultImage.Name = string.Empty;
			DisplayResult();
			this.ResumeLayout();
		}

		
		private void browseButton_Click(object sender, EventArgs e) {
			openFileDialog.Filter = "Image Files|*.Bmp;*.Emf;*.Exif;*.Gif;*.Ico;*.Jpg;*.Jpeg;*.Png;*.Tiff;*.Wmf|All files (*.*)|*.*";
			if (nameTextBox.Text != string.Empty)
				openFileDialog.InitialDirectory = Path.GetDirectoryName(nameTextBox.Text);
			
			if (openFileDialog.ShowDialog(this) == DialogResult.OK) {
				if (resultImage.Image != null) 
					resultImage.Image.Dispose();
				resultImage.Load(openFileDialog.FileName);
				if (string.IsNullOrEmpty(nameTextBox.Text))
					nameTextBox.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
				resultImage.Name = nameTextBox.Text;
				DisplayResult();
				this.ResumeLayout();
			}
		}


		private void nameTextBox_TextChanged(object sender, EventArgs e) {
			resultImage.Name = nameTextBox.Text;
		}


		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			DisplayResult();
		}


		protected override void OnFormClosed(FormClosedEventArgs e) {
			base.OnFormClosed(e);
			pictureBox.Image = null;
		}


		private void DisplayResult() {
			this.SuspendLayout();
			pictureBox.Image = resultImage.Image;
			nameTextBox.Text = resultImage.Name;
			this.ResumeLayout();
		}


		private NamedImage resultImage = new NamedImage();
	}
}