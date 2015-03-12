﻿using System;
using System.Windows.Forms;
using MW5.Api.Static;
using MW5.Services.Services.Abstract;

namespace MW5.Services.Services
{
    public class FileDialogService : IFileDialogService
    {
        private readonly IWin32Window _parent;

        public FileDialogService(IWin32Window parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            _parent = parent;
        }

        public bool SaveFile(string filter, out string filename)
        {
            filename = string.Empty;
            var dialog = new SaveFileDialog();
            dialog.Filter = filter;
            if (dialog.ShowDialog(_parent) == DialogResult.OK)
            {
                filename = dialog.FileName;
                return true;
            }
            return false;
        }

        public bool Open(string filter, out string filename)
        {
           filename = string.Empty;
            string[] filenames;
            if (OpenFileCore(filter, false, out filenames))
            {
                filename = filenames[0];
            }
            return filename != string.Empty;
        }

        public bool OpenFile(DataSourceType layerType, out string filename)
        {
            filename = string.Empty;
            string[] filenames;
            if (OpenFileCore(GetLayerFilter(layerType), false, out filenames))
            {
                filename = filenames[0];
            }
            return filename != string.Empty;
        }

        public bool OpenFiles(DataSourceType layerType, out string[] filenames)
        {
            return OpenFileCore(GetLayerFilter(layerType), true, out filenames);
        }

        public bool ChooseFolder(string initialPath, out string chosenPath)
        {
            chosenPath = string.Empty;
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                dialog.SelectedPath = initialPath;
                if (dialog.ShowDialog(_parent) == DialogResult.OK)
                {
                    chosenPath = dialog.SelectedPath;
                    return true;
                }
            }
            return false;
        }

        private bool OpenFileCore(string filter, bool multiSelect, out string[] filenames)
        {
            filenames = null;
            using (var dialog = new OpenFileDialog())
            {
                dialog.Multiselect = multiSelect;
                dialog.Filter = filter;
                if (dialog.ShowDialog(_parent) == DialogResult.OK)
                {
                    filenames = dialog.FileNames;
                }
            }
            return filenames != null;
        }

        private static string GetLayerFilter(DataSourceType layerType)
        {
            switch (layerType)
            {
                case DataSourceType.All:
                    return GeoSourceManager.FileFilter;
                case DataSourceType.Raster:
                    return GeoSourceManager.RasterFilter;
                case DataSourceType.Vector:
                    return GeoSourceManager.VectorFilter;
            }
            return "All files|*.*";
        }
    }
}