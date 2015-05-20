﻿using System;
using MW5.Api.Interfaces;
using MW5.Api.Legend.Abstract;
using MW5.Api.Legend.Events;
using MW5.Plugins.Identifier.Enums;
using MW5.Plugins.Identifier.Views;
using MW5.Plugins.Interfaces;

namespace MW5.Plugins.Identifier.Listeners
{
    public class MapListener
    {
        private readonly IdentifierPresenter _identifierPresenter;

        public MapListener(IAppContext context, IdentifierPlugin plugin, IdentifierPresenter identifierPresenter)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (identifierPresenter == null) throw new ArgumentNullException("identifierPresenter");

            _identifierPresenter = identifierPresenter;

            plugin.ShapeIdentified += _plugin_ShapeIdentified;
            plugin.ProjectClosed += plugin_ProjectClosed;
            plugin.LayerRemoved += plugin_LayerRemoved;
        }

        private void plugin_LayerRemoved(IMuteLegend legend, LayerEventArgs e)
        {
            _identifierPresenter.RemoveLayer(e.LayerHandle);
        }

        private void plugin_ProjectClosed(object sender, EventArgs e)
        {
            _identifierPresenter.RunCommand(IdentifierCommand.Clear);
        }

        private void _plugin_ShapeIdentified(IMuteMap map, Api.Events.ShapeIdentifiedEventArgs e)
        {
            _identifierPresenter.ShapeIdentified(e.LayerHandle, e.ShapeIndex);
        }
    }
}
