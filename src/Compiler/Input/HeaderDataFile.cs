﻿using System;
using System.Collections.Generic;

namespace Compiler.Input
{
    public class HeaderDataFile : AbstractSectorDataFile
    {
        public HeaderDataFile()
            : base(InputDataType.FILE_HEADERS)
        {

        }

        public override IEnumerator<SectorData> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
