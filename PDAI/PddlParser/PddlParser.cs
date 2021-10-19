﻿using Parser.Pddl.Internal;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Pddl
{
    public class PddlParser : IPddlParser
    {
        public Domain Parse(string domainFolderPath, int maxProblems = int.MaxValue)
        {
            var domainParser = new DomainParser();
            var problemParser = new ProblemParser();

            var domain = new Domain();
            domainParser.Parser(domainFolderPath + "/domain.pddl", domain);

            var problemsFolder = Directory.GetDirectories(domainFolderPath + "/runs/optimal");
            foreach (var folder in problemsFolder)
            {
                var problem = problemParser.Parse(folder);
                if(problem is not null)
                    domain.Problems.Add(problem);
            }

            return domain;
        }
    }
}
