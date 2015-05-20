﻿using System;
using System.Windows.Forms;
using MW5.Api.Enums;
using MW5.Api.Events;
using MW5.Plugins.Identifier.Enums;
using MW5.Plugins.Interfaces;
using MW5.Plugins.Mvp;

namespace MW5.Plugins.Identifier.Views
{
    public class IdentifierPresenter: CommandDispatcher<IIdentifierView, IdentifierCommand>, IDockPanelPresenter
    {
        private readonly IAppContext _context;

        public IdentifierPresenter(IAppContext context, IIdentifierView view): base(view)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;

            view.ModeChanged += OnIdentifierModeChanged;
            view.ShapeSelected += OnShapeSelected;
            view.PixelSelected += OnPixelSelected;
        }

        private void OnPixelSelected(object sender, Controls.RasterEventArgs e)
        {
            var shapes = _context.Map.IdentifiedShapes;
            shapes.Clear();
            shapes.AddPixel(e.LayerHandle, e.RasterX, e.RasterY);

            if (View.ZoomToShape)
            {
                var img = _context.Map.GetImage(e.LayerHandle);
                if (img != null)
                {
                    var bounds = img.GetPixelBounds(e.RasterX, e.RasterY);
                    bounds = bounds.Inflate(bounds.Width*10, bounds.Height*10);
                    _context.Map.ZoomToExtents(bounds);
                }
            }
            else
            {
                _context.Map.Redraw(RedrawType.SkipDataLayers);
            }
        }

        private void OnShapeSelected(object sender, Controls.ShapeEventArgs e)
        {
            var shapes = _context.Map.IdentifiedShapes;
            shapes.Clear();
            shapes.AddShape(e.LayerHandle, e.ShapeIndex);

            if (View.ZoomToShape)
            {
                _context.Map.ZoomToShape(e.LayerHandle, e.ShapeIndex);
            }
            else
            {
                _context.Map.Redraw(RedrawType.SkipDataLayers);    
            }
        }

        private void OnIdentifierModeChanged()
        {
            switch (View.Mode)
            {
                case IdentifierPluginMode.CurrentLayer:
                    _context.Map.Identifier.Mode = IdentifierMode.SingleLayer;
                    break;
                case IdentifierPluginMode.TopDownStopOnFirst:
                    _context.Map.Identifier.Mode = IdentifierMode.AllLayerStopOnFirst;
                    break;
                case IdentifierPluginMode.AllLayers:
                case IdentifierPluginMode.LayerSelection:
                    _context.Map.Identifier.Mode = IdentifierMode.AllLayers;
                    break;
            }
        }

        public void RemoveLayer(int layerHandle)
        {
            _context.Map.IdentifiedShapes.RemoveByLayerHandle(layerHandle);
            View.UpdateView();
        }

        public void ShapeIdentified(int layerHandle, int shapeIndex)
        {
            View.UpdateView();
        }

        public override void RunCommand(IdentifierCommand command)
        {
            switch (command)
            {
                case IdentifierCommand.Clear:
                    _context.Map.IdentifiedShapes.Clear();
                    View.Clear();
                    _context.Map.Redraw(RedrawType.SkipDataLayers);
                    break;
            }
        }

        public Control GetInternalObject()
        {
            return View as Control;
        }
    }
}
