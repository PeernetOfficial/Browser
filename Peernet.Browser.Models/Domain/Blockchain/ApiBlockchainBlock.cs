using System.Collections.Generic;

namespace Peernet.Browser.Models.Domain.Blockchain
{
    public class ApiBlockchainBlock
    {
        public BlockchainBlokStatus Status { get; set; }
        public string PeerId { get; set; }
        public byte[] LastBlockHash { get; set; }
        public ulong BlockchaninVersion { get; set; }
        public ulong Number { get; set; }

        public List<ApiBlockRecordRaw> RecordsRow { get; set; }

        public object RecordDecoded { get; set; }
    }
}