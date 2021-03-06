﻿using System.Linq;
using Regolith.Common;

namespace Regolith.Asteroids
{
    public class REGO_ModuleAsteroidAnalysis : PartModule
    {

        private Part _potato;

        [KSPField(guiActive = true, guiName = "", guiActiveEditor = false)]
        public string status = "No asteroid detected";

        public override void OnUpdate()
        {
            if (vessel != null)
            {
                if (_potato == null) FindAsteroidResources();
            }
        }

        private void FindAsteroidResources()
        {
            {
                var potatoes = vessel.Parts.Where(p => p.Modules.Contains("ModuleAsteroid"));
                if (potatoes.Any())
                {
                    if (_potato == null)
                    {
                        _potato = potatoes.FirstOrDefault();
                        var resTotal = 0f;
                        var resources = _potato.FindModulesImplementing<REGO_ModuleAsteroidResource>();
                        foreach (var res in resources)
                        {
                            var analysis =
                                part.FindModulesImplementing<REGO_ModuleAnalysisResource>().FirstOrDefault(r => r.resourceName == res.resourceName);
                            if (analysis != null)
                            {
                                analysis.abundance = res.abundance;
                                resTotal += analysis.abundance;
                            }
                        }
                        Fields["status"].guiName = "Rock";
                        status = string.Format("{0:0.0000}%", 100 - (resTotal * 100));

                    }
                    return;
                }
            }
            _potato = null;
        }
    }
}
