﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using xstrat.Core;
using static System.Net.Mime.MediaTypeNames;
using Path = System.IO.Path;

namespace xstrat.MVVM.View
{
    /// <summary>
    /// Interaction logic for ReplayView.xaml
    /// </summary>
    public partial class ReplayView : UserControl
    {

        public List<MatchReplayFolder> ReplayFolders { get; set; }

        public ReplayView()
        {
            InitializeComponent();
        }

        public void LoadReplays()
        {
            //Refresh List

            GenerateFolderList();

            //Update UI
        }

        #region FileHandling

        public void GenerateFolderList()
        {
            var list = new List<MatchReplayFolder>();

            string xstratpath = SettingsHandler.XStratReplayPath;
            string gamepath = SettingsHandler.GameReplayPath;

            if (gamepath.IsNullOrEmpty() || !Directory.Exists(gamepath))
            {
                Notify.sendWarn("Please set Game Replay Path in Settings");
                return;
            }
            if (xstratpath.IsNullOrEmpty())
            {
                Notify.sendWarn("Could not find XStrat Replay Folder: " + xstratpath);
                return;
            }
            Directory.CreateDirectory(xstratpath);

            //Get Saved Replays

            var xstratreplays = Directory.GetDirectories(xstratpath, "*", SearchOption.AllDirectories);
            foreach (var xreplay in xstratreplays)
            {
                //has rounds in it
                if (!hasRounds(xreplay)) continue;

                var rep = new MatchReplayFolder();

                string foldername = Path.GetDirectoryName(xreplay);

                rep.FolderName = foldername;
                rep.IsXStratPath = true;

                rep.JsonCreated = File.Exists(Path.Combine(xstratpath, foldername + ".json"));

                list.Add(rep);
            }

            //Get Game Folder Replays
            var gamereplays = Directory.GetDirectories(gamepath, "*", SearchOption.AllDirectories);
            foreach (var greplay in xstratreplays)
            {
                //has rounds in it
                if (!hasRounds(greplay)) continue;

                string foldername = Path.GetDirectoryName(greplay);
                
                MatchReplayFolder rep;

                rep = list.Where(x => x.FolderName == foldername).FirstOrDefault();

                bool needsAdd = false;

                if (rep == null)
                {
                    rep = new MatchReplayFolder();

                    rep.FolderName = foldername;
                    needsAdd = true;
                }

                rep.IsInGameFolder = true;

                rep.JsonCreated = File.Exists(Path.Combine(xstratpath, foldername + ".json"));

                if (needsAdd) list.Add(rep);
            }

            //Get Titles from XML
            var titles = GetTitleDict();
            if (GetTitleDict() == null) return;

            foreach (var title in titles)
            {
                var listItems = list.Where(x => x.Title == title.Key);
                foreach (var item in listItems)
                {
                    item.Title = title.Value;
                }
            }

            ReplayFolders = list;
        }

        public void ImportAll()
        {
            if(ReplayFolders == null) return;

            var toImport = ReplayFolders.Where(x => !x.IsXStratPath && x.IsInGameFolder).AsEnumerable();

            foreach (var item in toImport)
            {
                Import(item.FolderName);
            }
            LoadReplays();
        }

        public void Import(string folderName)
        {
            string xstratpath = SettingsHandler.XStratReplayPath;
            string gamepath = SettingsHandler.GameReplayPath;

            string sourceDirectory = Path.Combine(SettingsHandler.XStratReplayPath, folderName);
            string targetDirectory = Path.Combine(SettingsHandler.GameReplayPath, folderName);

            if (gamepath.IsNullOrEmpty() || !Directory.Exists(gamepath))
            {
                Notify.sendWarn("Please set Game Replay Path in Settings");
                return;
            }
            if (xstratpath.IsNullOrEmpty() || !Directory.Exists(xstratpath))
            {
                Notify.sendWarn("Could not find XStrat Replay Folder: " + xstratpath);
                return;
            }
            
            if(!Directory.Exists(sourceDirectory)) return;
            if (Directory.Exists(targetDirectory)) return;

            Globals.CopyFilesRecursively(sourceDirectory, targetDirectory);
        }

        #endregion

        #region FileHelpers
        public bool hasRounds(string path)
        {
            if (!Directory.Exists(path)) return false;
            var files = Directory.GetFiles(path, "*.rec", SearchOption.AllDirectories);

            return files.Length > 0;
        }

        public Dictionary<string, string> GetTitleDict()
        {
            string xmlFile = Path.Combine(SettingsHandler.XStratReplayPath, "ReplayTitles.xml");

            if (xmlFile.IsNullOrEmpty() || !File.Exists(xmlFile))
            {
                Logger.Log("Could not find existing replay file in: " + xmlFile);
                return null;
            }
            XmlSerializer deserializer = new XmlSerializer(typeof(Dictionary<string, string>));
            using (StringReader stringReader = new StringReader(xmlFile))
            {
                return (Dictionary<string, string>)deserializer.Deserialize(stringReader);                
            }
        }

        public void SaveTitleDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (ReplayFolders == null || ReplayFolders.Count == 0) return;

            foreach (var folder in ReplayFolders)
            {
                dict.Add(folder.FolderName, folder.Title);
            }

            SerializeTitleDict(dict);
        }

        public void SerializeTitleDict(Dictionary<string, string> dict)
        {
            string xmlFile = Path.Combine(SettingsHandler.XStratReplayPath, "ReplayTitles.xml");

            if (xmlFile.IsNullOrEmpty())
            {
                Logger.Log("XML Path is empty - could not save dictionary: " + xmlFile);
                return;
            }
            string xml = dict.SerializeObject();

            File.WriteAllText(xmlFile, xml);
        }
        #endregion
    }
    public class MatchReplayFolder
    {
        public string FolderName { get; set; }
        public string Title { get; set; }
        public bool IsInGameFolder{ get; set; }
        public bool IsXStratPath { get; set; }
        public bool JsonCreated { get; set; }
    }
}