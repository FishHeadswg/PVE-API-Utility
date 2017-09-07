/*
 * DocViewer.cs
 * Simple form-based document viewer.
 */

using System;
using System.Windows.Forms;

namespace PVEAPIUtility
{
    public partial class DocViewer : Form
    {
        public DocViewer()
        {
            InitializeComponent();
        }

        public void SetURL(string url)
        {
            webBrowser.Url = new Uri(url);
        }
    }
}