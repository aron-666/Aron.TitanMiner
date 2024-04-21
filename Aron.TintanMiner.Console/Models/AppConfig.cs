using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aron.TintanMiner.Console.Models
{
    public class AppConfig
    {
        public string? TITAN_NETWORK_LOCATORURL { get; set; } = "https://test-locator.titannet.io:5000/rpc/v0";

        public string? TITAN_EDGE_BINDING_URL { get; set; } = "https://api-test1.container1.titannet.io/api/v2/device/binding";

        public string? TITAN_EDGE_ID { get; set; } = "26DA6097-BAFC-45A5-8B08-A6DC441AE2E7";

        public int? TITAN_STORAGE_STORAGEGB { get; set; } = 20;

        public string? TITAN_STORAGE_PATH { get; set; } = "";

        public int? TITAN_CPU_CORES { get; set; } = 1;

        public int? TITAN_MEMORY_MEMORYGB { get; set; } = 1;
    }
}
