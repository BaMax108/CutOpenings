using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CutOpeningsPlugin.Inserting.Models
{
    /// <summary></summary>
    public class ActionsWithGroups
    {
        private readonly Document Doc;
        /// <summary></summary>
        public ActionsWithGroups(Document _doc) => Doc = _doc;

        /// <summary></summary>
        public void RotateUngroupDelete(Group group, RevitLinkInstance link)
        {
            RotateGroup(group.Id,
                GetAngleBeetwenDocAndDoclink(link.GetTotalTransform()),
                link.GetTotalTransform().Origin);
            string groupName = group.Name;
            Ungroup(group);
            DeleteGroup(groupName);
        }

        /// <summary>
        /// Создание группы
        /// </summary>
        public Group CreateGroup(ICollection<ElementId> ids)
        {
            if (ids.Count == 0) return null;
            Group group = null;
            string groupName = $"OpeningsGroup_{DateTime.Now:yyyy-MM-dd_HH.mm.ss}";
            using (Transaction t = new Transaction(Doc, "Create group"))
            {
                t.Start("Create group");
                group = Doc.Create.NewGroup(ids);
                group.GroupType.Name = groupName;
                t.Commit();
            }
            return group;
        }

        /// <summary>
        /// Изменение угла поворота группы
        /// </summary>
        public void RotateGroup(ElementId groupId, double angle, XYZ xyz)
        {
            using (Transaction t = new Transaction(Doc, "Group rotating"))
            {
                t.Start("Group rotating");
                ElementTransformUtils.RotateElement(Doc, groupId,
                    Line.CreateBound(xyz, xyz.Add(XYZ.BasisZ)),
                    angle);
                t.Commit();
            }
        }

        /// <summary>
        /// Разгруппирование
        /// </summary>
        public void Ungroup(Group group)
        {
            using (Transaction t = new Transaction(Doc, "Ungroup"))
            {
                t.Start("Ungroup");
                group.UngroupMembers();
                t.Commit();
            }


        }

        /// <summary>
        /// Удаление группы из диспетчера проекта
        /// </summary>
        public void DeleteGroup(string group)
        {
            foreach (var g in new FilteredElementCollector(Doc).OfClass(typeof(GroupType)).ToList().Where(x => x.Name == group))
            {
                if (g.Name == group)
                {
                    using (Transaction t = new Transaction(Doc, "Delete group"))
                    {
                        t.Start("Delete group");
                        Doc.Delete(g.Id);
                        t.Commit();
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Получение угла между одноименными осями координатных систем открытого документа и связанного файла
        /// </summary>
        public double GetAngleBeetwenDocAndDoclink(Transform t)
        {
            return Math.PI * 2 - XYZ.BasisX.AngleTo(t.BasisX);
        }
    }
}
