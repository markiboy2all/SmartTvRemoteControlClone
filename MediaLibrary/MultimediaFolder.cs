// Decompiled with JetBrains decompiler
// Type: MediaLibrary.DataModels.MultimediaFolder
// Assembly: MediaLibrary, Version=1.1.0.22848, Culture=neutral, PublicKeyToken=null
// MVID: C1095F1F-5B5F-4F7A-8339-F069AE56C21B
// Assembly location: C:\Users\Mark\Desktop\Samsung-RemoteControl-master\Drivers\MediaLibrary.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MediaLibrary.DataModels
{
  [XmlRoot("root")]
  public class MultimediaFolder : ItemBase
  {
    [XmlArray("itemsList")]
    [XmlArrayItem("item")]
    public List<ItemBase> ItemsList { get; set; }

    public MultimediaFolder()
    {
    }

    public MultimediaFolder(string name, Guid parent, DateTime date)
    {
      this.Name = name;
      this.Parent = parent;
      this.ItemsList = new List<ItemBase>();
      this.Date = date;
      this.ID = Guid.NewGuid();
      this.Type = ItemType.Folder;
    }

    public void AddItem(ItemBase item) => this.ItemsList.Add(item);

    public void RemoveItem(ItemBase item)
    {
      if (!this.ItemsList.Contains(item))
        return;
      this.ItemsList.Remove(item);
    }

    public MultimediaFolder FindFolderByID(Guid ID)
    {
      if (this.ID == ID)
        return this;
      foreach (ItemBase items in this.ItemsList)
      {
        if (items.Type == ItemType.Folder)
        {
          MultimediaFolder multimediaFolder = (MultimediaFolder) items;
          if (multimediaFolder.ID == ID)
            return multimediaFolder;
          multimediaFolder.FindFolderByID(ID);
        }
      }
      return (MultimediaFolder) null;
    }

    public ItemBase FindItemById(Guid id)
    {
      ItemBase itemBase = this.ItemsList.FirstOrDefault<ItemBase>((Func<ItemBase, bool>) (i => i.ID == id));
      if (itemBase != null)
        return itemBase;
      foreach (ItemBase items in this.ItemsList)
      {
        if (items.Type == ItemType.Folder)
        {
          ItemBase itemById = ((MultimediaFolder) items).FindItemById(id);
          if (itemById != null)
            return itemById;
        }
      }
      return (ItemBase) null;
    }

    public List<Content> GetFilesList() => this.ItemsList.Where<ItemBase>((Func<ItemBase, bool>) (i => i.Type != ItemType.Folder)).Select<ItemBase, Content>((Func<ItemBase, Content>) (i => i as Content)).ToList<Content>();

    public List<ItemBase> GetFoldersList() => this.ItemsList.Where<ItemBase>((Func<ItemBase, bool>) (i => i.Type == ItemType.Folder)).ToList<ItemBase>();

    public List<Content> GetAllFilesList(List<Content> fileslist = null)
    {
      if (fileslist == null)
        fileslist = new List<Content>();
      foreach (ItemBase items in this.ItemsList)
      {
        switch (items.Type)
        {
          case ItemType.Content:
            fileslist.Add((Content) items);
            continue;
          case ItemType.Folder:
            ((MultimediaFolder) items).GetAllFilesList(fileslist);
            continue;
          default:
            continue;
        }
      }
      return fileslist;
    }
  }
}
