﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MW5.Plugins.Concrete;
using MW5.Plugins.Enums;
using MW5.Plugins.Interfaces;
using MW5.Projections.UI.Forms;
using MW5.Tools.Enums;
using MW5.Tools.Model;
using MW5.Tools.Properties;

namespace MW5.Tools.Tools.Projections
{
    [GisTool(GroupKeys.Projections)]
    public class IdentifyProjectionTool: GisToolBase
    {
        private IAppContext _context;

        /// <summary>
        /// The name of the tool.
        /// </summary>
        public override string Name
        {
            get { return "Identify projection";  }
        }

        /// <summary>
        /// Description of the tool.
        /// </summary>
        public override string Description
        {
            get { return "Identifies projection as one of the well known from the projection string provided by user."; }
        }

        /// <summary>
        /// Runs the tool.
        /// </summary>
        public override bool Run(CancellationToken token)
        {
            using (var form = new IdentifyProjectionForm(_context))
            {
                return _context.View.ShowChildView(form);
            }
        }

        /// <summary>
        /// Initializes the tool.
        /// </summary>
        public override void Initialize(IAppContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
        }
    }
}
