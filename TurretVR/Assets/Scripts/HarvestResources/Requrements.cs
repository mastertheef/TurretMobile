using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.HarvestResources
{
    [Serializable]
    public class Requrements
    {
        public ResourceEnum Resource;
        public List<ResourceDTO> Requirement;
    }
}
