﻿using System.Numerics;
using System.Reflection;
using ImGuiNET;
using QuickImGuiNET;
using RetroMole.Core;
using Serilog;
using Tomlyn;
using Tomlyn.Model;
using bk = QuickImGuiNET.Veldrid;

namespace RetroMole;

public static class Launch
{
    private static Backend _backend;
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .MinimumLevel.Debug()
            .CreateLogger();
        Log.Information("RetroMole Launch v0.1");
        
        // Config Sinks/Sources
        var cfg_toml = new Config.Toml("RetroMole.cfg", ref _backend);
        var cfg_cli = new Config.Cli(args, ref _backend);
        
        // Shadow Backend
        Log.Logger.Information("Initializing Veldrid Backend (shadow)");
        _backend = new bk.Backend
        {
            // Add Config to shadow backend
            Config = new Config
            {
                _default = new TomlTable
                {
                    { "window", new TomlTable {
                        { "width", 1280 },
                        { "height", 720 }
                    }},

                    { "veldrid", new TomlTable {
                        {"backend", -1 }
                    }},

                    { "serilog", new TomlTable {
                        { "minimumLevel", "Debug" },
                        { "using", new TomlTable {
                            { "Console", "Serilog.Sinks.Console" },
                            { "File", "Serilog.Sinks.File" },
                        }},
                        { "writeTo", new TomlTable {
                            { "Console", new TomlTable {
                                { "outputTemplate", "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}" }
                            }},
                            { "File", new TomlTable {
                                { "outputTemplate", "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}" },
                                { "path", "RetroMole.log" },
                                { "fileSizeLimitBytes", "2000000" },
                                { "rollOnFileSizeLimit", "true" },
                                { "retainedFileCountLimit", "10" },
                                { "rollingInterval", "Day" }
                            }}
                        }}
                    }},
                    {"paths", new TomlTable {
                            {"home", (Environment.OSVersion.Platform is PlatformID.Unix or PlatformID.MacOSX
                                    ? Environment.GetEnvironmentVariable("HOME")
                                    : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%"
                                    )) ?? string.Empty 
                            },
                            { "temp", Path.GetFullPath(Path.Combine(Path.GetTempPath(), "RetroMoleTemp")) },
                            { "paths.exec", Path.GetFullPath(
                                Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location ?? Assembly.GetExecutingAssembly()?.Location)
                                ?? Path.Combine(Path.GetTempPath(), "RetroMoleTemp", "tmpxc")
                            )}
                    }}
                },
                Sinks = new IConfigSink[]
                {
                    cfg_toml,
                    cfg_cli
                },
                Sources = new IConfigSource[]
                {
                    cfg_toml,
                    cfg_cli
                }
            },
            // Add default Logger to shadow backend
            Logger = Log.Logger,

            // Add Events to shadow backend
            Events = new Dictionary<string, Event>
            {
                {
                    "onMainMenuBar", new Event(new Dictionary<string, Event>
                    {
                        { "Debug", new Event() }
                    })
                },
                { "widgetReg", new Event() }
            },

            // Add Widget Registry to shadow backend
            WidgetReg = new Dictionary<string, Widget>()
        };
        
        // Loading config sources
        _backend.Logger.Information($"Loading default config + ({_backend.Config.Sources.Length}) source(s)");
        _backend.Config.LoadDefault();
        _backend.Config.From(_backend.Config.Sources[0]);
        _backend.Config.From(_backend.Config.Sources[1]);

        _backend.Logger.Information("Initializing new logger using config");
        Log.CloseAndFlush();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.KeyValuePairs(((TomlTable)_backend.Config["serilog"]).SelectMany(kvp =>
                kvp.Key switch
                {
                    "using" => ((TomlTable)kvp.Value).Select(use =>
                        new KeyValuePair<string, string>($"using:{use.Key}", (string)use.Value)),
                    "writeTo" => ((TomlTable)kvp.Value).SelectMany(sink =>
                        ((TomlTable)sink.Value).Select(o =>
                            new KeyValuePair<string, string>($"write-to:{sink.Key}.{o.Key}", (string)o.Value))),
                    _ => new[] { new KeyValuePair<string, string>(kvp.Key, (string)kvp.Value) }
                }
            )).CreateLogger();
        _backend.Logger = Log.Logger;

        // Initialize shadow -> ready backend
        _backend.Logger.Information("Config Done, fully initializing shadow backend");
        _backend.Init();
        
        // Initialize RetroMole UI
        var gui = new Gui(_backend);
        
        //TODO Load Packages
        _backend.Logger.Information("Load Packages (todo)");

        // Run backend loop and handle errors
        _backend.Logger.Information("Run Backend loop");
        try
        {
            _backend.Run(gui.Draw, UpdateCallback: gui.Update);
        }
        catch (Exception e)
        {
            _backend.Logger.Error(e.ToString());
        }
        finally
        {
            _backend.Logger.Information($"EXITING: {Environment.ExitCode}");
            Log.CloseAndFlush();
        }
    }
}