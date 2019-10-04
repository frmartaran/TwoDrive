using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.BusinessLogic.LogicInput;
using Microsoft.EntityFrameworkCore;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class FolderLogicTest
    {
        private Folder root;

        [TestInitialize]
        public void SetUp()
        {
            var writer = new Writer();
            root = new Folder
            {
                Id = 1,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Root",
                Owner = writer,
                FolderChildren = new List<Element>()
            };

        }

        [TestMethod]
        public void CreateFolder()
        {
            var mockFolderRepository = new Mock<IRepository<Folder>>(MockBehavior.Strict);
            mockFolderRepository.Setup(m => m.Insert(It.IsAny<Folder>()));
            mockFolderRepository.Setup(m => m.Save());

            var mockFileRepository = new Mock<IRepository<File>>(MockBehavior.Strict);
            mockFileRepository.Setup(m => m.Insert(It.IsAny<File>()));
            mockFileRepository.Setup(m => m.Save());

            var mockElementValidator = new Mock<IValidator<Element>>(MockBehavior.Strict);
            mockElementValidator.Setup(m => m.IsValid(It.IsAny<Element>()))
            .Returns(true);

            var folderLogicDependencies = new FolderLogicDependencies
            {
                FolderRepository = mockFolderRepository.Object,
                FileRepository = mockFileRepository.Object,
                ElementValidator = mockElementValidator.Object
            };

            var logic = new FolderLogic(folderLogicDependencies);
            logic.Create(root);

            mockFolderRepository.VerifyAll();
            mockElementValidator.VerifyAll();
        }

        [TestMethod]
        public void CreateFolderCheckState()
        {
            var context = ContextFactory.GetMemoryContext("Create Folder Check State");
            var folderRepository = new FolderRepository(context);
            var folderValidator = new FolderValidator();
            var folderLogicDependencies = new FolderLogicDependencies
            {
                FolderRepository = folderRepository,
                ElementValidator = folderValidator
            };
            var logic = new FolderLogic(folderLogicDependencies);

            logic.Create(root);

            var folderinDb = folderRepository.Get(1);
            Assert.AreEqual(root, folderinDb);
        }

        [TestMethod]
        public void CreateParentFolderWithTwoChildren()
        {
            var context = ContextFactory.GetMemoryContext("Create Test 2");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);

            folderRepository.Insert(root);
            folderRepository.Save();
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var file = new TxtFile
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                Content = "Content"
            };
            root.FolderChildren.Add(child);
            folderRepository.Insert(child);
            fileRepository.Insert(file);
            folderRepository.Update(root);
            folderRepository.Save();

            var allFoldersInDb = folderRepository.GetAll();
            var allFilesInDb = fileRepository.GetAll();

            Assert.IsTrue(allFoldersInDb.Contains(root));
            Assert.IsTrue(allFoldersInDb.Contains(child));
            Assert.IsTrue(allFilesInDb.Contains(file));
        }

        [TestMethod]
        public void CreateParentFolderWithTwoLevelsOfChildren()
        {
            var context = ContextFactory.GetMemoryContext("Create Test 3");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);

            folderRepository.Insert(root);
            folderRepository.Save();
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var secondChild = new Folder
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child 2",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var grandson = new Folder
            {
                Id = 4,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child 3",
                Owner = root.Owner,
                ParentFolder = child,
                FolderChildren = new List<Element>()
            };
            root.FolderChildren.Add(child);
            child.FolderChildren.Add(grandson);
            folderRepository.Insert(child);
            folderRepository.Insert(secondChild);
            folderRepository.Update(root);
            folderRepository.Save();

            var allFoldersInDb = folderRepository.GetAll();
            Assert.IsTrue(allFoldersInDb.Contains(root));
            Assert.IsTrue(allFoldersInDb.Contains(child));
            Assert.IsTrue(allFoldersInDb.Contains(secondChild));
            Assert.IsTrue(allFoldersInDb.Contains(grandson));
        }

        [TestMethod]
        public void DeleteFolder()
        {
            var mockModificationRepository = new Mock<IRepository<Modification>>(MockBehavior.Strict);
            var mockFolderRepository = new Mock<IRepository<Folder>>(MockBehavior.Strict);
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new Mock<IValidator<Element>>().Object,
                FileRepository = new Mock<IRepository<File>>().Object,
                FolderRepository = mockFolderRepository.Object,
                ModificationRepository = mockModificationRepository.Object

            };
            mockFolderRepository.Setup(m => m.Delete(It.IsAny<int>()));
            mockFolderRepository
            .Setup(m => m.Get(It.IsAny<int>()))
            .Returns(root);
            mockFolderRepository.Setup(m => m.Save());

            mockModificationRepository.Setup(m => m.Insert(It.IsAny<Modification>()));
            mockModificationRepository.Setup(m => m.Save());

            var mockFileRepository = new Mock<IRepository<File>>(MockBehavior.Strict);

            var logic = new FolderLogic(dependecies);
            logic.Delete(root.Id);

            mockFolderRepository.VerifyAll();
            mockModificationRepository.VerifyAll();
        }

        [TestMethod]
        public void DeleteOneFolder()
        {
            var context = ContextFactory.GetMemoryContext("Delete One Folder");
            var modificationRepository = new ModificationRepository(context);
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new Mock<IValidator<Element>>().Object,
                FileRepository = fileRepository,
                FolderRepository = folderRepository,
                ModificationRepository = modificationRepository

            };
            folderRepository.Insert(root);
            folderRepository.Save();

            var logic = new FolderLogic(dependecies);
            logic.Delete(root.Id);

            var rootInDb = folderRepository.Get(1);
            var modifications = modificationRepository.GetAll().Count;
            Assert.IsNull(rootInDb);
            Assert.AreEqual(1, modifications);
        }

        [TestMethod]
        public void DeleteParentFolder()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 1");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var modificationRepository = new ModificationRepository(context);
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new Mock<IValidator<Element>>().Object,
                FileRepository = fileRepository,
                FolderRepository = folderRepository,
                ModificationRepository = modificationRepository

            };
            folderRepository.Insert(root);
            folderRepository.Save();
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            root.FolderChildren.Add(child);
            folderRepository.Insert(child);
            folderRepository.Update(root);
            folderRepository.Save();

            var logic = new FolderLogic(dependecies);
            logic.Delete(root.Id);

            var modifications = modificationRepository.GetAll().Count;
            var allFoldersInDb = folderRepository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
            Assert.AreEqual(2, modifications);
        }

        [TestMethod]
        public void DeleteParentWithTwoChildren()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 2");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var modificationRepository = new ModificationRepository(context);
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new Mock<IValidator<Element>>().Object,
                FileRepository = fileRepository,
                FolderRepository = folderRepository,
                ModificationRepository = modificationRepository

            };
            folderRepository.Insert(root);
            folderRepository.Save();
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var secondChild = new Folder
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child 2",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            root.FolderChildren.Add(child);
            folderRepository.Insert(child);
            folderRepository.Insert(secondChild);
            folderRepository.Update(root);
            folderRepository.Save();

            var logic = new FolderLogic(dependecies);
            logic.Delete(root.Id);

            var modifications = modificationRepository.GetAll().Count;
            var allFoldersInDb = folderRepository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
            Assert.AreEqual(3, modifications);
        }

        [TestMethod]
        public void DeleteParentWithAFolderAndFile()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 3");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var modificationRepository = new ModificationRepository(context);
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new Mock<IValidator<Element>>().Object,
                FileRepository = fileRepository,
                FolderRepository = folderRepository,
                ModificationRepository = modificationRepository

            };
            folderRepository.Insert(root);
            folderRepository.Save();
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var file = new TxtFile
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                Content = "Content"
            };
            root.FolderChildren.Add(child);
            folderRepository.Insert(child);
            fileRepository.Insert(file);
            folderRepository.Update(root);
            folderRepository.Save();

            var logic = new FolderLogic(dependecies);
            logic.Delete(root.Id);

            var modifications = modificationRepository.GetAll().Count;
            var allFoldersInDb = folderRepository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
            Assert.AreEqual(3, modifications);
        }

        [TestMethod]
        public void DeleteParentWithTwoLevelsOfChildren()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 4");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var modificationRepository = new ModificationRepository(context);
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new Mock<IValidator<Element>>().Object,
                FileRepository = fileRepository,
                FolderRepository = folderRepository,
                ModificationRepository = modificationRepository

            };
            folderRepository.Insert(root);
            folderRepository.Save();
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var secondChild = new Folder
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child 2",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var grandson = new Folder
            {
                Id = 4,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child 3",
                Owner = root.Owner,
                ParentFolder = child,
                FolderChildren = new List<Element>()
            };
            root.FolderChildren.Add(child);
            child.FolderChildren.Add(grandson);
            folderRepository.Insert(child);
            folderRepository.Insert(secondChild);
            folderRepository.Update(root);
            folderRepository.Save();

            var logic = new FolderLogic(dependecies);
            logic.Delete(root.Id);

            var modifications = modificationRepository.GetAll().Count;
            var allFoldersInDb = folderRepository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
            Assert.AreEqual(4, modifications);
        }

        [TestMethod]
        public void UpdateFolder()
        {
            var mockFolderRepository = new Mock<IRepository<Folder>>(MockBehavior.Strict);
            var mockFolderValidator = new Mock<IValidator<Element>>(MockBehavior.Strict);
            var mockFileRepository = new Mock<IRepository<File>>(MockBehavior.Strict);

            var folderLogicDependencies = new FolderLogicDependencies
            {
                FolderRepository = mockFolderRepository.Object,
                FileRepository = mockFileRepository.Object,
                ElementValidator = mockFolderValidator.Object
            };

            mockFolderRepository.Setup(m => m.Update(It.IsAny<Folder>()));
            mockFolderRepository.Setup(m => m.Save());
            mockFolderValidator.Setup(m => m.IsValid(It.IsAny<Element>()))
            .Returns(true);

            root.Name = "Root 2.0";
            var logic = new FolderLogic(folderLogicDependencies);
            logic.Update(root);
            mockFolderRepository.VerifyAll();
            mockFolderValidator.VerifyAll();

        }

        [TestMethod]
        public void UpdateFolderCheckState()
        {
            var context = ContextFactory.GetMemoryContext("Update Folder Check State");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var folderValidator = new FolderValidator();

            folderRepository.Insert(root);
            folderRepository.Save();

            var folderLogicDependencies = new FolderLogicDependencies
            {
                FolderRepository = folderRepository,
                FileRepository = fileRepository,
                ElementValidator = folderValidator
            };

            var newOwner = new Writer();

            var dateModified = root.DateModified;
            root.Owner = newOwner;
            var logic = new FolderLogic(folderLogicDependencies);
            logic.Update(root);

            var folderInDb = folderRepository.Get(1);

            Assert.AreNotEqual(dateModified, folderInDb.DateModified);
            Assert.AreEqual(newOwner, folderInDb.Owner);
        }

        [TestMethod]
        public void GetAll()
        {
            var testList = new List<Folder>();
            var mockFolderRepository = new Mock<IRepository<Folder>>(MockBehavior.Strict);
            mockFolderRepository.Setup(m => m.GetAll())
            .Returns(testList);

            var mockFileRepository = new Mock<IRepository<File>>(MockBehavior.Strict);

            var logic = new FolderLogic(mockFolderRepository.Object, mockFileRepository.Object);
            var elements = logic.GetAll();

            mockFolderRepository.VerifyAll();
        }

        [TestMethod]
        public void GetAllFolders()
        {
            var context = ContextFactory.GetMemoryContext("Get All test");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);

            var newFolder = new Folder
            {
                Id = 2
            };
            var file = new TxtFile
            {
                Id = 3
            };
            folderRepository.Insert(root);
            folderRepository.Insert(newFolder);
            fileRepository.Insert(file);
            folderRepository.Save();

            var logic = new FolderLogic(folderRepository, fileRepository);
            var allFoldersInDb = logic.GetAll();
            Assert.IsTrue(allFoldersInDb.Contains(root));
            Assert.IsTrue(allFoldersInDb.Contains(newFolder));
            Assert.AreEqual(2, allFoldersInDb.Count);

        }

        [TestMethod]
        public void Get()
        {
            var mockFolderRepository = new Mock<IRepository<Folder>>(MockBehavior.Strict);
            var mockFileRepository = new Mock<IRepository<File>>(MockBehavior.Strict);
            mockFolderRepository.Setup(m => m.Get(It.IsAny<int>()))
                        .Returns(root);

            var logic = new FolderLogic(mockFolderRepository.Object, mockFileRepository.Object);
            var folder = logic.Get(1);

            mockFolderRepository.VerifyAll();
        }

        [TestMethod]
        public void GetFolder()
        {
            var context = ContextFactory.GetMemoryContext("Get Folder");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);

            folderRepository.Insert(root);
            folderRepository.Save();

            var logic = new FolderLogic(folderRepository, fileRepository);
            var folderinDb = logic.Get(1);

            Assert.AreEqual(root, folderinDb);
        }

        [TestMethod]
        public void ShowTreeOneFolder()
        {
            var context = ContextFactory.GetMemoryContext("Show One Tree");
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new Mock<IValidator<Element>>().Object,
                FolderRepository = new FolderRepository(context),
                FileRepository = new Mock<IRepository<File>>().Object,
            };
            dependecies.FolderRepository.Insert(root);
            dependecies.FolderRepository.Save();
            var logic = new FolderLogic(dependecies);
            var tree = logic.ShowTree(root);

            Assert.AreEqual(" +- Root \n", tree);
        }

        [TestMethod]
        public void ShowTreeTwoFolders()
        {
            var context = ContextFactory.GetMemoryContext("Show Two Folders Tree");
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new Mock<IValidator<Element>>().Object,
                FolderRepository = new FolderRepository(context),
                FileRepository = new Mock<IRepository<File>>().Object,
            };
            var logic = new FolderLogic(dependecies);
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            dependecies.FolderRepository.Insert(root);
            dependecies.FolderRepository.Insert(child);
            dependecies.FolderRepository.Save();
            var testList = new List<Element>();
            testList.Add(child);
            root.FolderChildren = testList;
            var tree = logic.ShowTree(root);

            var prefix = "      ";
            var expectedString = string.Format("{0} +- {1} \n", "", root.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix}\\", child.Name);
            Assert.AreEqual(expectedString, tree);
        }

        [TestMethod]
        public void ShowTreeFolderAndFile()
        {
            var context = ContextFactory.GetMemoryContext("Show Folder and Child Tree");
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new Mock<IValidator<Element>>().Object,
                FolderRepository = new FolderRepository(context),
                FileRepository = new FileRepository(context),
            };
            var logic = new FolderLogic(dependecies);
            var file = new TxtFile
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                Content = "Content"
            };
            dependecies.FolderRepository.Insert(root);
            dependecies.FileRepository.Insert(file);
            dependecies.FileRepository.Save();
            var testList = new List<Element>();
            testList.Add(file);
            root.FolderChildren = testList;
            var tree = logic.ShowTree(root);
            var prefix = "      ";
            var expectedString = string.Format("{0} +- {1} \n", "", root.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix}\\", file.Name);
            Assert.AreEqual(expectedString, tree);
        }

        [TestMethod]
        public void ShowTreeTwoChildFolders()
        {
            var context = ContextFactory.GetMemoryContext("Show Two Child Tree");
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new Mock<IValidator<Element>>().Object,
                FolderRepository = new FolderRepository(context),
                FileRepository = new Mock<IRepository<File>>().Object,
            };
            var logic = new FolderLogic(dependecies);
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var secondChild = new Folder
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            dependecies.FolderRepository.Insert(root);
            dependecies.FolderRepository.Insert(child);
            dependecies.FolderRepository.Insert(secondChild);
            dependecies.FolderRepository.Save();
            var testList = new List<Element>();
            testList.Add(child);
            testList.Add(secondChild);
            root.FolderChildren = testList;
            var tree = logic.ShowTree(root);

            var prefix = "      ";
            var expectedString = string.Format("{0} +- {1} \n", "", root.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix}|", child.Name);
            expectedString += string.Format("{0} +- {1} \n", "      \\", secondChild.Name);
            Assert.AreEqual(expectedString, tree);
        }

        [TestMethod]
        public void ShowTreeOfThreeLevels()
        {
            var context = ContextFactory.GetMemoryContext("Show Three level Tree");
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new FolderValidator(),
                FolderRepository = new FolderRepository(context),
                FileRepository = new FileRepository(context),
            };
            var logic = new FolderLogic(dependecies);
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "First Child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var file = new TxtFile
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "File",
                Owner = root.Owner,
                ParentFolder = child,
                Content = "Content"
            };
            child.FolderChildren.Add(file);
            var secondChild = new Folder
            {
                Id = 4,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Second Child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            dependecies.FolderRepository.Insert(root);
            dependecies.FolderRepository.Insert(child);
            dependecies.FolderRepository.Insert(secondChild);
            dependecies.FileRepository.Insert(file);
            dependecies.FolderRepository.Save();
            var testList = new List<Element>();
            testList.Add(child);
            testList.Add(secondChild);
            root.FolderChildren = testList;
            var tree = logic.ShowTree(root);
            var prefix = "      ";
            var expectedString = string.Format("{0} +- {1} \n", "", root.Name);
            expectedString += string.Format("{0} +- {1} \n", "      |", child.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix}\\", file.Name);
            expectedString += string.Format("{0} +- {1} \n", "      \\", secondChild.Name);
            Assert.AreEqual(expectedString, tree);
        }

        [TestMethod]
        public void ShowComplexTree()
        {
            var context = ContextFactory.GetMemoryContext("Show Complex Tree");
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new FolderValidator(),
                FolderRepository = new FolderRepository(context),
                FileRepository = new FileRepository(context),
            };
            var logic = new FolderLogic(dependecies);
            var child = new Folder
            {
                Id = 2,
                Name = "First Child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var FirstGrandson = new Folder
            {
                Id = 3,
                Name = "Grandson 1",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var SecondGrandson = new Folder
            {
                Id = 4,
                Name = "Grandson 2",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var SecondGrandsonFile = new TxtFile
            {
                Id = 5,
                Name = "Grandson Child 1",
                Owner = root.Owner,
                ParentFolder = SecondGrandson,
            };
            var SecondGrandsonFileTwo = new TxtFile
            {
                Id = 6,
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson Child 2",
                Owner = root.Owner,
                ParentFolder = SecondGrandson,
            };
            SecondGrandson.FolderChildren.Add(SecondGrandsonFile);
            var ThirdGrandson = new Folder
            {
                Id = 7,
                Name = "Grandson 3",
                Owner = root.Owner,
                ParentFolder = child,
                FolderChildren = new List<Element>()
            };
            var file = new TxtFile
            {
                Id = 8,
                Name = "Grandson 4",
                Owner = root.Owner,
                ParentFolder = child,
                Content = "Content"
            };
            child.FolderChildren.Add(FirstGrandson);
            child.FolderChildren.Add(SecondGrandson);
            child.FolderChildren.Add(ThirdGrandson);
            child.FolderChildren.Add(file);
            var secondChild = new Folder
            {
                Id = 9,
                Name = "Second Child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };

            var fileOne = new TxtFile
            {
                Id = 10,
                Name = "Grandson 5",
                Owner = root.Owner,
                ParentFolder = secondChild,
            };
            var fileTwo = new TxtFile
            {
                Id = 11,
                Name = "Grandson 6",
                Owner = root.Owner,
                ParentFolder = secondChild,
            };
            var fileThree = new TxtFile
            {
                Id = 12,
                Name = "Grandson 7",
                Owner = root.Owner,
                ParentFolder = secondChild,
            };
            var fileFour = new TxtFile
            {
                Id = 13,
                Name = "Grandson 8",
                Owner = root.Owner,
                ParentFolder = secondChild,
            };
            var AnotherFolder = new Folder
            {
                Id = 14,
                Name = "Grandson 9",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var fileFive = new TxtFile
            {
                Id = 15,
                Name = "Grandson 9 Child",
                Owner = root.Owner,
                ParentFolder = AnotherFolder,
            };
            AnotherFolder.FolderChildren.Add(fileFive);
            secondChild.FolderChildren.Add(fileOne);
            secondChild.FolderChildren.Add(fileTwo);
            secondChild.FolderChildren.Add(fileThree);
            secondChild.FolderChildren.Add(fileFour);
            secondChild.FolderChildren.Add(AnotherFolder);
            dependecies.FolderRepository.Insert(root);
            dependecies.FolderRepository.Insert(child);
            dependecies.FolderRepository.Insert(secondChild);
            dependecies.FolderRepository.Insert(FirstGrandson);
            dependecies.FolderRepository.Insert(SecondGrandson);
            dependecies.FolderRepository.Insert(ThirdGrandson);
            dependecies.FolderRepository.Insert(AnotherFolder);
            dependecies.FileRepository.Insert(file);
            dependecies.FileRepository.Insert(fileOne);
            dependecies.FileRepository.Insert(fileTwo);
            dependecies.FileRepository.Insert(fileThree);
            dependecies.FileRepository.Insert(fileFour);
            dependecies.FileRepository.Insert(fileFive);
            dependecies.FileRepository.Insert(SecondGrandsonFile);
            dependecies.FileRepository.Insert(SecondGrandsonFileTwo);
            dependecies.FolderRepository.Save();
            var testList = new List<Element>();
            testList.Add(child);
            testList.Add(secondChild);
            root.FolderChildren = testList;
            var tree = logic.ShowTree(root);
            var prefix = "      ";
            var expectedString = string.Format("{0} +- {1} \n", "", root.Name);
            expectedString += string.Format("{0} +- {1} \n", "      |", child.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix}|", FirstGrandson.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix}|", SecondGrandson.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix + prefix}|", SecondGrandsonFile.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix + prefix}\\", SecondGrandsonFileTwo.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix}|", ThirdGrandson.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix}\\", file.Name);
            expectedString += string.Format("{0} +- {1} \n", "      \\", secondChild.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix}|", fileOne.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix}|", fileTwo.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix}|", fileThree.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix}|", fileFour.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix}\\", AnotherFolder.Name);
            expectedString += string.Format("{0} +- {1} \n", $"{prefix + prefix + prefix}\\", fileFive.Name);
            Assert.AreEqual(expectedString, tree);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteNullFolder()
        {
            var context = ContextFactory.GetMemoryContext("Show Complex Tree");
            var dependecies = new FolderLogicDependencies
            {
                ElementValidator = new FolderValidator(),
                FolderRepository = new FolderRepository(context),
                FileRepository = new FileRepository(context),
            };
            var logic = new FolderLogic(dependecies);
            logic.Delete(1);
        }
    }
}