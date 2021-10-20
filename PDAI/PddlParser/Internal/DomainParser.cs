﻿using PddlParser.Internal;
using Shared.ExtensionMethods;
using Shared.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Pddl.Internal
{
    internal class DomainParser
    {
        internal void Parser(string filePath, Domain domain)
        {
            domain.Entities = GetTypes(filePath);
            domain.Actions = GetActions(filePath, domain.Entities);
            domain.Predicates = GetPredicates(filePath, domain.Entities);
        }

        private List<Predicate> GetPredicates(string filePath, List<Entity> entities)
        {
            Logger.Log("Loading Predicated");
            List<string> lines = File.ReadAllLines(filePath).ToList();
            return PredicatesParser.Parse(lines, entities);
        }

        private List<Action> GetActions(string filePath, List<Entity> entities)
        {
            Logger.Log("Loading Actions");
            List<string> lines = File.ReadAllLines(filePath).ToList();
            return ActionsParser.Parse(lines, entities);
        }

        private List<Entity> GetTypes(string filePath)
        {
            Logger.Log("Loading Types");
            List<string> lines = File.ReadAllLines(filePath).ToList();
            return TypesParser.Parse(lines);
        }
    }
}
