﻿using SharpDox.Model;
using System.IO;

namespace SharpDox.Build.Context.Step
{
    internal class CheckConfigStep : StepBase
    {
        public CheckConfigStep(StepInput stepInput, int progressStart, int progressEnd) :
            base(stepInput, stepInput.SDBuildStrings.StepCheckConfig, new StepRange(progressStart, progressEnd)) { }

        public override SDProject RunStep(SDProject sdProject)
        {
            if (string.IsNullOrEmpty(_stepInput.CoreConfigSection.ProjectName))
                throw new SDBuildException(_stepInput.SDBuildStrings.NoProjectNameGiven);

            if (string.IsNullOrEmpty(_stepInput.CoreConfigSection.InputFile))
                throw new SDBuildException(_stepInput.SDBuildStrings.NoProjectGiven);

            if (!File.Exists(_stepInput.CoreConfigSection.InputFile))
                throw new SDBuildException(_stepInput.SDBuildStrings.ProjectNotFound);

            return sdProject;
        }
    }
}
