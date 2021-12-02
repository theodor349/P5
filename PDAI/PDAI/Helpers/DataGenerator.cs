﻿using System.Collections.Generic;
using System.IO;
using PddlParser.Internal;
using Shared;
using Shared.ExtensionMethods;

namespace PDAI.Helpers
{
    internal abstract class DataGenerator
    {
        internal readonly Settings _settings;
        private readonly DataGenerationHelper _dataGenHelper;

        internal bool runActionsInParallel = true;
        internal bool runSplitsInParallel = true;

        internal bool isFirstIteration => iteration == 0;
        internal int iteration = 0;

        public DataGenerator(Settings settings)
        {
            _settings = settings;
            _dataGenHelper = new DataGenerationHelper(_settings);

            Run();
        }

        internal abstract void Run();

        internal void SetInput(string action)
        {
            ConstraintHelper ch = new ConstraintHelper();
            List<string> trainingFolders = SystemExtensions.GetTrainingFolders(action);

            foreach (var trainingFolder in trainingFolders)
            {
                ch.IncrementConstraintValues(Path.Combine(trainingFolder, "bias.pl"), 0, 0, 2);
            }
        }

        internal void Train(string action)
        {
            var trainingFolders = SystemExtensions.GetTrainingFolders(action);
            SystemExtensions.RunnInParallel(trainingFolders, x => SetInput(x), _settings.Cores, false);
            SystemExtensions.RunnInParallel(trainingFolders, x => RunPopper(x), _settings.Cores, runSplitsInParallel);
        }

        internal void Test(string action)
        {
            var trainingFolders = SystemExtensions.GetTrainingFolders(action);
            SystemExtensions.RunnInParallel(trainingFolders, x => _dataGenHelper.RunTest(x), _settings.Cores);
        }

        internal void SaveResults(string action)
        {
            var trainingFolders = SystemExtensions.GetTrainingFolders(action);
            foreach (var trainingFolder in trainingFolders)
                TempFileManager.SaveStats(_settings.TargetFolder, trainingFolder);
        }

        private void RunPopper(string trainPath)
        {
            _dataGenHelper.RunPopper(trainPath, _settings.TargetFolder, _settings.Beta, _settings.MaxRuntime);
        }
    }
}
