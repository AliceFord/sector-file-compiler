﻿using Compiler.Argument;
using Compiler.Input;
using Compiler.Output;
using System.Collections.Generic;
using Compiler.Model;
using Compiler.Event;
using Newtonsoft.Json;
using Compiler.Validate;
using Compiler.Parser;
using Compiler.Config;
using Compiler.Error;
using Compiler.Exception;
using System;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Compiler
{
    public class SectorFileCompiler
    {
        private readonly CompilerArguments arguments;
        private readonly EventTracker events;

        public SectorFileCompiler(CompilerArguments arguments, EventTracker events)
        {
            this.arguments = arguments;
            this.events = events;
        }

        /**
         * Run the compiler.
         */
        public int Compile()
        {
            this.events.AddEvent(new ComplilationStartedEvent());

            CompilerArgumentsValidator.Validate(this.events, this.arguments);
            if (this.events.HasFatalError())
            {
                this.events.AddEvent(new CompilationFinishedEvent(false));
                return 1;
            }

            // Parse all the config files
            OutputGroupRepository outputGroups = new OutputGroupRepository();
            // TODO: Remove this and uncomment below
            SectorElementCollection sectorElements = new SectorElementCollection();
            (new OutputGenerator()).GenerateOutput(sectorElements, outputGroups, new StreamWriter("outputtest.txt"));
            return 1;


            ConfigFileLoader fileLoader = new ConfigFileLoader(outputGroups);

            ConfigFileList fileList;
            try
            {
                fileList = fileLoader.LoadConfigFiles(this.arguments.ConfigFiles);
            } catch (ConfigFileInvalidException e)
            {
                this.events.AddEvent(new CompilationMessage(e.Message));
                this.events.AddEvent(new CompilationFinishedEvent(false));
                return 1;
            }

            // Parse all the input files
            // TODO: UNCOMMENT
            //SectorElementCollection sectorElements = new SectorElementCollection();
            DataParserFactory parserFactory = new DataParserFactory(sectorElements, events);
            foreach (AbstractSectorDataFile dataFile in fileList)
            {
                parserFactory.GetParserForFile(dataFile).ParseData(dataFile);
            }

            if (this.events.HasFatalError())
            {
                this.events.AddEvent(new CompilationFinishedEvent(false));
                return 1;
            }

            // Validate the output files
            if (this.arguments.ValidateOutput)
            {
                OutputValidator.Validate(sectorElements, this.arguments, this.events);
                if (this.events.HasFatalError())
                {
                    this.events.AddEvent(new CompilationFinishedEvent(false));
                    return 1;
                }
            } 
            else
            {
                this.events.AddEvent(new CompilationMessage("Skipping output validation"));
            }

            // Perform the compilation
            //CompileEngineFactory.Create(arguments, sectorElements).Compile();

            this.events.AddEvent(new CompilationFinishedEvent(true));
            return 0;
        }
    }
}
