﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Shared.Models
{
    public class Action : Clause
    {
        private Regex paramReg = new Regex(@":parameters.((\w*.)*)");
        private Regex parentReg = new Regex(@"\(.*\)");
        private Regex multipleTypeReg = new Regex(@"(\?\w*.)*-.\w*");
        private Regex singleTypeParenReg = new Regex(@":parameters\s\(([^()]*)\)");

        public Action()
        {

        }

        public Action(string text, List<Entity> entities)
        {
            Name = text.Split(" ")[0].Trim();

            if (entities.Count > 1)
            {
                HandleMultipleTypes(text, entities);
            }
            else
            {
                HandleSingleType(text, entities);
            }

        }

        private void HandleSingleType(string text, List<Entity> entities)
        {
            string parameter = paramReg.Match(text).Value;
            string parameterParentacy = singleTypeParenReg.Match(parameter).Value;
            var matchTypes = multipleTypeReg.Matches(parameterParentacy);

        }

        private void HandleMultipleTypes(string text, List<Entity> entities)
        {
            var matchTypes = multipleTypeReg.Matches(parentReg.Match(paramReg.Match(text).Value).Value);

            foreach (Match match in matchTypes)
            {
                var split = match.Value.Split("-");
                var t = entities.Where(x => x.Type.Equals(split[1].Trim())).FirstOrDefault();

                Parameters.AddRange(GetParameters(split[0], t));
            }
        }

        private List<Parameter> GetParameters(string text, Entity entity)
        {
            var res = new List<Parameter>();
            var words = text.Split("?");
            foreach (var word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                res.Add(new Parameter()
                {
                    Name = word.Trim(),
                    Entity = entity,
                });
            }
            return res;
        }
    }
}
