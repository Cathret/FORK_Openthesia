﻿using IconFonts;
using ImGuiNET;
using System.Numerics;

namespace Openthesia;

public class MidiFileView
{
    public static void Render()
    {
        ImGui.BeginChild("Midi file view", ImGui.GetContentRegionAvail(), ImGuiChildFlags.None, ImGuiWindowFlags.NoScrollWithMouse);

        ImGui.PushFont(FontController.Font16_Icon16);
        ImGui.SetCursorScreenPos(new Vector2(22, 50) * FontController.DSF);
        if (ImGui.Button(FontAwesome6.ArrowLeftLong, new Vector2(100, 50) * FontController.DSF))
        {
            Router.SetRoute(Router.Routes.MidiList);
        }
        ImGui.PopFont();

        ImGuiTheme.Style.Colors[(int)ImGuiCol.ChildBg] = Settings.MainBg * 0.8f;

        ImGui.SetNextWindowPos(new((ImGui.GetIO().DisplaySize.X - ImGui.GetIO().DisplaySize.X / 1.2f) / 2, 120 * FontController.DSF));
        if (ImGui.BeginChild("Container", new(ImGui.GetIO().DisplaySize.X / 1.2f, ImGui.GetIO().DisplaySize.Y / 1.2f),
            ImGuiChildFlags.AlwaysUseWindowPadding | ImGuiChildFlags.Border))
        {
            Drawings.RenderMatrixBackground();

            ImGui.PushFont(FontController.Title);
            ImGui.SetCursorPos(new((ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize(MidiFileData.FileName.Replace(".mid", string.Empty)).X) / 2, 50 * FontController.DSF));
            ImGui.Text(MidiFileData.FileName.Replace(".mid", string.Empty));
            ImGui.PopFont();

            ImGui.PushFont(FontController.BigIcon);
            ImGui.SetCursorPos(new(ImGui.GetIO().DisplaySize.X / 10 + 125 * FontController.DSF - ImGui.CalcTextSize(FontAwesome6.Music).X / 2, ImGui.GetIO().DisplaySize.Y / 2.5f));
            ImGui.Text(FontAwesome6.Music);
            ImGui.PopFont();

            ImGui.PushFont(FontController.BigIcon);
            ImGui.SetCursorPos(new(ImGui.GetIO().DisplaySize.X / 2.8f + 125 * FontController.DSF - ImGui.CalcTextSize(FontAwesome6.Gamepad).X / 2, ImGui.GetIO().DisplaySize.Y / 2.5f));
            ImGui.Text(FontAwesome6.Gamepad);
            ImGui.PopFont();

            ImGui.PushFont(FontController.BigIcon);
            ImGui.SetCursorPos(new(ImGui.GetIO().DisplaySize.X / 1.6f + 125 * FontController.DSF - ImGui.CalcTextSize(FontAwesome6.Hands).X / 2, ImGui.GetIO().DisplaySize.Y / 2.5f));
            ImGui.Text(FontAwesome6.Hands);
            ImGui.PopFont();

            ImGui.PushFont(FontController.GetFontOfSize(22));

            var drawList = ImGui.GetWindowDrawList();
            string t1 = "Peacefully listen and visualize the piece";
            drawList.AddText(new(ImGui.GetIO().DisplaySize.X / 10 + 125 * FontController.DSF, ImGui.GetIO().DisplaySize.Y / 2.5f),
                ImGui.GetColorU32(Vector4.One), t1);
            string t2 = "Playback will wait for the right note input";
            drawList.AddText(new(ImGui.GetIO().DisplaySize.X / 2.8f + 125 * FontController.DSF, ImGui.GetIO().DisplaySize.Y / 2.5f),
                ImGui.GetColorU32(Vector4.One), t2);
            string t3 = "Separate right and left hands with colors";
            drawList.AddText(new(ImGui.GetIO().DisplaySize.X / 1.6f + 125 * FontController.DSF, ImGui.GetIO().DisplaySize.Y / 2.5f),
                ImGui.GetColorU32(Vector4.One), t3);

            ImGuiTheme.PushButton(ImGuiTheme.HtmlToVec4("#31CB15"), ImGuiTheme.HtmlToVec4("#20870E"), ImGuiTheme.HtmlToVec4("#31CB15"));
            ImGui.SetCursorPos(new(ImGui.GetIO().DisplaySize.X / 10, ImGui.GetIO().DisplaySize.Y / 1.5f));
            if (ImGui.Button($"View and listen", new Vector2(250, 100) * FontController.DSF))
            {
                ScreenCanvas.SetLearningMode(false);
                ScreenCanvas.SetEditMode(false);
                LeftRightData.S_IsRightNote.Clear();
                foreach (var n in MidiFileData.Notes)
                {
                    LeftRightData.S_IsRightNote.Add(true);
                }
                MidiEditing.ReadData();
                Router.SetRoute(Router.Routes.MidiPlayback);
            }
            ImGuiTheme.PopButton();

            ImGuiTheme.PushButton(ImGuiTheme.HtmlToVec4("#0EA5E9"), ImGuiTheme.HtmlToVec4("#096E9B"), ImGuiTheme.HtmlToVec4("#0EA5E9"));
            ImGui.SetCursorPos(new(ImGui.GetIO().DisplaySize.X / 2.8f, ImGui.GetIO().DisplaySize.Y / 1.5f));
            if (ImGui.Button($"Play along", new Vector2(250, 100) * FontController.DSF))
            {
                MidiPlayer.Playback.Speed = 1;
                ScreenCanvas.SetFallSpeed(ScreenCanvas.FallSpeeds.Default);
                MidiPlayer.Playback.OutputDevice = null; // mute the playback
                ScreenCanvas.SetLearningMode(true);
                ScreenCanvas.SetEditMode(false);
                LeftRightData.S_IsRightNote.Clear();
                foreach (var n in MidiFileData.Notes)
                {
                    LeftRightData.S_IsRightNote.Add(true);
                }
                MidiEditing.ReadData();
                Router.SetRoute(Router.Routes.MidiPlayback);
            }
            ImGuiTheme.PopButton();

            ImGuiTheme.PushButton(ImGuiTheme.HtmlToVec4("#772525"), ImGuiTheme.HtmlToVec4("#4F1818"), ImGuiTheme.HtmlToVec4("#772525"));
            ImGui.SetCursorPos(new(ImGui.GetIO().DisplaySize.X / 1.6f, ImGui.GetIO().DisplaySize.Y / 1.5f));
            if (ImGui.Button($"Edit mode", new Vector2(250, 100) * FontController.DSF))
            {
                ScreenCanvas.SetLearningMode(false);
                ScreenCanvas.SetEditMode(true);
                LeftRightData.S_IsRightNote.Clear();
                foreach (var n in MidiFileData.Notes)
                {
                    LeftRightData.S_IsRightNote.Add(true);
                }
                MidiEditing.ReadData();
                Router.SetRoute(Router.Routes.MidiPlayback);
            }
            ImGuiTheme.PopButton();

            ImGui.PopFont();
            ImGui.EndChild();
        }
        ImGuiTheme.Style.Colors[(int)ImGuiCol.ChildBg] = Vector4.Zero;
        ImGui.EndChild();
    }
}
