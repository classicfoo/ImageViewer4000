/*
 * Created by SharpDevelop.
 * User: mike
 * Date: 30/10/2016
 * Time: 1:06 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using SimpletonUnitTestFramework;

namespace ImageViewer
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		string[] images;
		
		int _currentImageIndex = 0;
		
		//Enable unit testing
		SimpletonUnitTestFramework.MainForm tester;
		
		public MainForm(string[] args)
		{
			//After all the widgets load ...
			InitializeComponent();
			
			//Initialize tester ...
			tester = new SimpletonUnitTestFramework.MainForm();
			tester.Show();
			
			//Register the events ...
			//this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.This_MainFormKeyDown);
			this.pictureBox1.MouseEnter += new System.EventHandler(this.PictureBox1_MouseEnter);
			this.pictureBox1.MouseWheel += new MouseEventHandler(PictureBox1_MouseWheel);
			
			//Get image files in dir
		    var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };
		    var searchFolder = ".";
		    var isRecursive = false;
			List<String> filesFound = new List<String>();
		    var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
		    foreach (var filter in filters) {
		       filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
		    }
		    images = filesFound.ToArray();
		    
		    tester.Assert("all images are shown", 3, images.Length);
		    tester.Assert("index starts with 0", 0, _currentImageIndex);
		    			
			This_ShowImage(images, _currentImageIndex);

		}
		
//		 public static String[] Helper_GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
//		 {
//		 	Console.WriteLine("Getting Files ... ");
//		    List<String> filesFound = new List<String>();
//		    var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
//		    foreach (var filter in filters)
//		    {
//		       filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
//		    }
//		    return filesFound.ToArray();
//		 }

		void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
		{
			//PictureBox1_OnScrollUp
			if(e.Delta > 0) {
				
				//For testing purposes
				var previousImageIndex = _currentImageIndex;
				
				//Try previous image
				_currentImageIndex--;
				
				//if scroll reverse past image start, go last image
				if(_currentImageIndex < 0) { _currentImageIndex = images.Length -1; }	
		
				This_ShowImage(images, _currentImageIndex);
				
				//This_ShowNextImage(images, _currentImageIndex);
				
				tester.Assert("picturebox scrolls to next image", _currentImageIndex +1, previousImageIndex);
			}
			
			//PictureBox1_OnScrollDown
			if(e.Delta < 0) {
				
				//try next image
				_currentImageIndex++;

				//if scroll forward past image end, go back to first image
				if(_currentImageIndex > images.Length -1) { _currentImageIndex = 0; }
				
				This_ShowImage(images, _currentImageIndex);
				//This_ShowNextImage(images, _currentImageIndex);
			}
		}
		
//		void PictureBox1_OnMouseWheelDown(object sender, MouseEventArgs e)
//		{	
//				_currentImageIndex--;
//				This_ShowNextImage(images, _currentImageIndex);
//		}
//		
//		void PictureBox1_OnMouseWheelUp(object sender, MouseEventArgs e)
//		{
//				_currentImageIndex++;
//				This_ShowNextImage(images, _currentImageIndex);
//		}
		
//		void This_ShowNextImage(string[] images, int index)
//		{
//				
////				if(index > images.Length -1)
////				{
////					_currentImageIndex = 0;
////				}
////				
////				if(index < 0)
////				{
////					_currentImageIndex = images.Length -1;
////				}
//				
////				//if scroll forward past image end, go back to first image
////				if(index > images.Length -1) { _currentImageIndex = 0; }
////				
////				//if scroll reversepast image start, go last image
////				if(index < 0) {	_currentImageIndex = images.Length -1; }	
////				
//				//This_ShowImage(images, _currentImageIndex);
//				
//		}

		public int WrapNext(int start, int end, int current)
		{
				if(current > end)
				{
					current = start;
				}
				
				if(current < 0)
				{
					current = end;
				}
		}
		
		void This_ShowImage(string[] images, int index)
		{
		
			//prevent stuff up when scrolling too fast
			if(index <= images.Length -1 && index >= 0)
			{
				pictureBox1.ImageLocation = images[index];
				this.Text = "ImageViewer4000 - ";
				//Image count in titlebar
				//this.Text += (_currentImageIndex + 1) + "/" + images.Length + " - ";
				this.Text += Path.GetFileName(images[_currentImageIndex]);
			}
		}
		
//		void This_MainFormKeyDown(object sender, KeyEventArgs e)
//		{
//			if(e.KeyCode == Keys.Escape)
//			{
//				this.Close();
//			}
//		}
		
		//enable mousemove events
		void PictureBox1_MouseEnter(object sender, EventArgs e)
		{
			pictureBox1.Focus();
		}
	}
}
