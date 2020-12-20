﻿using System.Collections.Generic;
using Compiler.Event;
using Compiler.Input;
using Compiler.Model;
using Compiler.Parser;
using CompilerTest.Mock;
using Xunit;

namespace CompilerTest.Parser
{
    public abstract class AbstractParserTestCase
    {
        protected const string INPUT_FILE_NAME = "TEST/test.txt";
        protected readonly SectorElementCollection sectorElementCollection;
        protected readonly Moq.Mock<IEventLogger> logger;
        protected abstract InputDataType GetInputDataType();

        protected AbstractParserTestCase()
        {
            this.logger = new Moq.Mock<IEventLogger>();
            this.sectorElementCollection = new SectorElementCollection();
        }
        
        private AbstractSectorDataFile GetInputFile(List<string> lines)
        {
            return new SectorDataFileFactory(
                new MockInputStreamFactory(lines)
            ).Create(INPUT_FILE_NAME, GetInputDataType());
        }

        protected void RunParserOnLines(List<string> lines)
        {
            AbstractSectorDataFile file = this.GetInputFile(lines);
            new DataParserFactory(this.sectorElementCollection, this.logger.Object).GetParserForFile(file).ParseData(file);
        }

        protected void AssertExpectedComment(string expected, Comment actual)
        {
            Assert.Equal(expected, actual.CommentString);
        }

        /**
         * Line numbers start at one as that's more human friendly
         */
        protected void AssertExpectedMetadata(
            AbstractCompilableElement element,
            int definitionLineNumber,
            string commentString = "",
            List<string> docblockLines = null
        )
        {
            this.AssertExpectedComment(commentString, element.InlineComment);
            this.AssertExpectedDefinition(element.GetDefinition(), definitionLineNumber);
            this.AssertExpectedDocblockLines(element.Docblock, docblockLines);
        }
        
        protected void AssertExpectedDefinition(Definition definition, int lineNumber)
        {
            Assert.Equal(new Definition(INPUT_FILE_NAME, lineNumber), definition);
        }

        protected void AssertExpectedDocblockLines(Docblock docblock, List<string> docblockLines)
        {
            Docblock expectedDocblock = new Docblock();
            docblockLines?.ForEach(line => docblock.AddLine(new Comment(line)));

            Assert.Equal(expectedDocblock, docblock);
        }
    }
}
