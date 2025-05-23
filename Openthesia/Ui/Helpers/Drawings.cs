﻿using ImGuiNET;
using Melanchall.DryWetMidi.Interaction;
using Openthesia.Enums;
using Openthesia.Settings;
using System.Numerics;

namespace Openthesia.Ui.Helpers;

public class Drawings
{
    public static IntPtr C;
    public static IntPtr CSharp;
    public static IntPtr CSharpWhite;
    public static IntPtr SustainPedalOff;
    public static IntPtr SustainPedalOn;

    public static void RenderMatrixBackground()
    {
        var drawList = ImGui.GetWindowDrawList();
        var randomW = new Random();
        var randomH = new Random();
        var randomL = new Random();

        for (int i = 0; i < 20; i++)
        {
            int rw = randomW.Next(0, (int)ImGui.GetIO().DisplaySize.X);
            int rh = randomH.Next(0, (int)ImGui.GetIO().DisplaySize.Y);
            int rl = randomL.Next(10, 50);

            if (CoreSettings.NeonFx)
            {
                // Draw glowing effect
                for (int j = 0; j < 3; j++)
                {
                    float thickness = j * 2;
                    float alpha = 0.2f + (3 - j) * 0.2f;
                    uint color = ImGui.GetColorU32(new Vector4(ThemeManager.RightHandCol.X, ThemeManager.RightHandCol.Y, ThemeManager.RightHandCol.Z, alpha) * 0.5f);
                    drawList.AddRect(
                        new Vector2(rw - 1, rh - 1),
                        new Vector2(rw + 20 + 1, rh + rl + 1),
                        color,
                        5f,
                        0,
                        thickness
                    );
                }
            }
            drawList.AddRectFilled(new Vector2(rw, rh), new Vector2(rw + 20, rh + rl), ImGui.GetColorU32(ThemeManager.RightHandCol), 5, ImDrawFlags.RoundCornersAll);
        }
    }

    public static void Tooltip(string description)
    {
        if (ImGui.IsItemHovered(ImGuiHoveredFlags.None))
        {
            ImGui.BeginTooltip();
            ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
            ImGui.TextUnformatted(description);
            ImGui.PopTextWrapPos();
            ImGui.EndTooltip();
        }
    }

    public static void NoteTooltip(string description)
    {
        ImGui.BeginTooltip();
        ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
        ImGui.TextUnformatted(description);
        ImGui.PopTextWrapPos();
        ImGui.EndTooltip();
    }

    public static string GetNoteTextAs(TextTypes textType, Note note)
    {
        switch (textType)
        {
            case TextTypes.NoteName:
                return note.NoteName.ToString();
            case TextTypes.Velocity:
                return note.Velocity.ToString();
            case TextTypes.Octave:
                return note.Octave.ToString();
            default:
                return note.NoteName.ToString();
        }
    }
}
