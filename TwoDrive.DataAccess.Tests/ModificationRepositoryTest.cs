
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess.Tests
{
    [TestClass]
    public class ModificationRepositoryTest
    {
        [TestMethod]
        public void CreateModification()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 1");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var modification = new Modification
            {
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();

            var modificationInDb = context.Modifications.ToList().Count;
            Assert.AreEqual(1, modificationInDb);
        }

        [TestMethod]
        public void UpdateModification()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 2");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var modification = new Modification
            {
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();
            modification.type = ModificationType.Changed;
            repository.Update(modification);
            repository.Save();

            var modificationInDb = context.Modifications.FirstOrDefault();
            Assert.AreEqual(ModificationType.Changed, modificationInDb.type);
        }

        [TestMethod]
        public void DeleteModification()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 3");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var modification = new Modification
            {
                Id = 1,
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();

            repository.Delete(1);
            repository.Save();

            var modificationInDb = context.Modifications.ToList().Count;
            Assert.AreEqual(0, modificationInDb);
        }

        [TestMethod]
        public void GetModification()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 4");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var modification = new Modification
            {
                Id = 1,
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();

            var modificationInDb = repository.Get(1);
            Assert.AreEqual(modification, modificationInDb);
        }

        [TestMethod]
        public void GetAllModifications()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 5");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var modification = new Modification
            {
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();

            var modificationInDb = repository.GetAll();
            Assert.AreEqual(1, modificationInDb.Count);
            Assert.IsTrue(modificationInDb.Contains(modification));
        }

        [TestMethod]
        public void ExistsModification()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 6");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var modification = new Modification
            {
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();

            var exists = repository.Exists(modification);
            Assert.IsTrue(exists);
        }
    }
}