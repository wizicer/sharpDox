using System;
using System.Collections.Generic;
using System.Windows.Media;
using SharpDox.Model;
using SharpDox.UML.Extensions;
using SharpDox.UML.Sequence.Elements;
using SharpDox.UML.SVG;

namespace SharpDox.UML.Sequence.Model
{
    internal class SequenceDiagram : SequenceDiagramComposite, ISDDiagram
    {
        private readonly SequenceDiagramPngRenderer _sequenceDiagramPngRenderer;
        private readonly SequenceDiagramSvgRenderer _sequenceDiagramSvgRenderer;
        private readonly SDProject _sdProject;

        private DrawingVisual _renderedDiagram;
        private SvgRoot _renderedSvgDiagram;

        public SequenceDiagram(SDProject sdProject)
        {
            _sdProject = sdProject;
            _sequenceDiagramPngRenderer = new SequenceDiagramPngRenderer();
            _sequenceDiagramSvgRenderer = new SequenceDiagramSvgRenderer();

            Nodes = new List<SequenceDiagramNode>();
        }

        public SequenceDiagramElement AddNode(string typeIdentifier)
        {
            var node = new SequenceDiagramNode
            {
                TypeIdentifier = typeIdentifier,
                Text = _sdProject.GetTypeByIdentifier(typeIdentifier).Fullname
            };
            Nodes.Add(node);

            return node;
        }

        public void ToPng(string outputFilepath)
        {
            if (_renderedDiagram == null)
            {
                _renderedDiagram = _sequenceDiagramPngRenderer.RenderDiagram(this);
            }
            _renderedDiagram.SaveAsPng(outputFilepath);
        }

        public string ToSvg()
        {
            _renderedSvgDiagram = _sequenceDiagramSvgRenderer.RenderDiagram(this);
            return _renderedSvgDiagram.ToString();
        }

        public Guid StartNodeID { get; set; }
        public List<SequenceDiagramNode> Nodes { get; set; }
    }
}
