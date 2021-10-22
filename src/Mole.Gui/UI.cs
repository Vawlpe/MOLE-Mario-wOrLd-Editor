﻿using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mole.Gui.Windows;
using Mole.Shared;
using Num = System.Numerics;

namespace Mole.Gui
{
    [SuppressMessage("ReSharper", "PositionalPropertyUsedProblem")]
    public class Ui
    {
        private static readonly List<Window> Windows = new() {
           new About(),
           //new FileDialog(),
           //new RomInfo(),
           //new WGfx()
        };

        private static bool _showDemo;
        private static UiData _data = new();

        public class UiData
        {
            public Rom Rom;
            public Gfx Gfx;
            public string Path;
        }

        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        public static void Draw()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 6);
            ImGui.PushStyleVar(ImGuiStyleVar.PopupRounding, 6);
            ImGui.PushStyleVar(ImGuiStyleVar.GrabRounding, 6);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
            ImGui.PushStyleVar(ImGuiStyleVar.PopupBorderSize, 0);
            ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 0);
            
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Open ROM", "Ctrl+O"))
                        Windows[1].ShouldDraw = true;
                        
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Debug"))
                {
                    ImGui.MenuItem("Demo Window", null, ref _showDemo);
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Help"))
                {
                    if (ImGui.MenuItem("About", null))
                        Windows[0].ShouldDraw = true;

                    ImGui.EndMenu();
                }
                    
                ImGui.EndMainMenuBar();
            }

            if (_showDemo) ImGui.ShowDemoWindow(ref _showDemo);

            foreach (var w in Windows)
                w.Draw(_data, Windows);
        }
    }
}
