﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SharpDox.UML.Class
{
    internal class ClassDiagramRow
    {
        public ClassDiagramRow(string identifier, string type, string accessibility, string text)
        {
            Identifier = identifier;
            Accessibility = accessibility;
            Text = text;
            Type = type;
        }

        public string Identifier { get; set; }
        public string Accessibility { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
    }
}
