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
            var mockFolderRepository = new Mock<IFolderRepository>(MockBehavior.Strict);
            mockFolderRepository.Setup(m => m.Insert(It.IsAny<Folder>()));
            mockFolderRepository.Setup(m => m.Save());

            var mockFileRepository = new Mock<IFileRepository>(MockBehavior.Strict);
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
            var context = ContextFactory.GetMemoryContext("Create Test");
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
            var mockFolderRepository = new Mock<IFolderRepository>(MockBehavior.Strict);
            mockFolderRepository.Setup(m => m.Delete(It.IsAny<int>()));
            mockFolderRepository
            .Setup(m => m.Get(It.IsAny<int>()))
            .Returns(root);
            mockFolderRepository.Setup(m => m.Save());

            var mockFileRepository = new Mock<IFileRepository>(MockBehavior.Strict);

            var logic = new FolderLogic(mockFolderRepository.Object, mockFileRepository.Object);
            logic.Delete(root.Id);

            mockFolderRepository.VerifyAll();
        }

        [TestMethod]
        public void DeleteOneFolder()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);

            folderRepository.Insert(root);
            folderRepository.Save();

            var logic = new FolderLogic(folderRepository, fileRepository);
            logic.Delete(root.Id);

            var rootInDb = folderRepository.Get(1);
            Assert.IsNull(rootInDb);
        }

        [TestMethod]
        public void DeleteParentFolder()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 1");
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
            root.FolderChildren.Add(child);
            folderRepository.Insert(child);
            folderRepository.Update(root);
            folderRepository.Save();

            var logic = new FolderLogic(folderRepository, fileRepository);
            logic.Delete(root.Id);

            var allFoldersInDb = folderRepository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
        }

        [TestMethod]
        public void DeleteParentWithTwoChildren()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 2");
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
            root.FolderChildren.Add(child);
            folderRepository.Insert(child);
            folderRepository.Insert(secondChild);
            folderRepository.Update(root);
            folderRepository.Save();

            var logic = new FolderLogic(folderRepository, fileRepository);
            logic.Delete(root.Id);

            var allFoldersInDb = folderRepository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
        }

        [TestMethod]
        public void DeleteParentWithAFolderAndFile()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 3");
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

            var logic = new FolderLogic(folderRepository, fileRepository);
            logic.Delete(root.Id);

            var allFoldersInDb = folderRepository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
        }

        [TestMethod]
        public void DeleteParentWithTwoLevelsOfChildren()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 4");
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

            var logic = new FolderLogic(folderRepository, fileRepository);
            logic.Delete(root.Id);

            var allFoldersInDb = folderRepository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
        }

        [TestMethod]
        public void UpdateFolder()
        {
            var mockFolderRepository = new Mock<IFolderRepository>(MockBehavior.Strict);
            var mockFolderValidator = new Mock<IValidator<Element>>(MockBehavior.Strict);
            var mockFileRepository = new Mock<IFileRepository>(MockBehavior.Strict);

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
            var context = ContextFactory.GetMemoryContext("Update Test");
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
            var mockFolderRepository = new Mock<IFolderRepository>(MockBehavior.Strict);
            mockFolderRepository.Setup(m => m.GetAll())
            .Returns(testList);

            var mockFileRepository = new Mock<IFileRepository>(MockBehavior.Strict);

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
            var mockFolderRepository = new Mock<IFolderRepository>(MockBehavior.Strict);
            var mockFileRepository = new Mock<IFileRepository>(MockBehavior.Strict);
            mockFolderRepository.Setup(m => m.Get(It.IsAny<int>()))
                        .Returns(root);

            var logic = new FolderLogic(mockFolderRepository.Object, mockFileRepository.Object);
            var folder = logic.Get(1);

            mockFolderRepository.VerifyAll();
        }

        [TestMethod]
        public void GetFolder()
        {
            var context = ContextFactory.GetMemoryContext("Get test");
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
            var mockDependecies = new Mock<FolderLogicDependencies>(MockBehavior.Strict);
            var logic = new FolderLogic(mockDependecies.Object);
            var tree = logic.ShowTree(root);

            Assert.AreEqual(" +- Root \n", tree);
        }

        [TestMethod]
        public void ShowTreeTwoFolders()
        {
            var mockDependecies = new Mock<FolderLogicDependencies>(MockBehavior.Strict);
            var logic = new FolderLogic(mockDependecies.Object);
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
            var mockDependecies = new Mock<FolderLogicDependencies>(MockBehavior.Strict);
            var logic = new FolderLogic(mockDependecies.Object);
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
            var mockDependecies = new Mock<FolderLogicDependencies>(MockBehavior.Strict);
            var logic = new FolderLogic(mockDependecies.Object);
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
            var mockDependecies = new Mock<FolderLogicDependencies>(MockBehavior.Strict);
            var logic = new FolderLogic(mockDependecies.Object);
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
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Second Child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
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
            var mockDependecies = new Mock<FolderLogicDependencies>(MockBehavior.Strict);
            var logic = new FolderLogic(mockDependecies.Object);
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
            var FirstGrandson = new Folder
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson 1",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var SecondGrandson = new Folder
            {
                Id = 4,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson 2",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var SecondGrandsonFile = new TxtFile
            {
                Id = 5,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson Child 1",
                Owner = root.Owner,
                ParentFolder = SecondGrandson,
                Content = "Content"
            };var SecondGrandsonFileTwo = new TxtFile
            {
                Id = 6,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson Child 2",
                Owner = root.Owner,
                ParentFolder = SecondGrandson,
                Content = "Content"
            };
            SecondGrandson.FolderChildren.Add(SecondGrandsonFile);
            SecondGrandson.FolderChildren.Add(SecondGrandsonFileTwo);
            var ThirdGrandson = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson 3",
                Owner = root.Owner,
                ParentFolder = child,
                FolderChildren = new List<Element>()
            };
            var file = new TxtFile
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
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
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Second Child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            
            var fileOne = new TxtFile
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson 5",
                Owner = root.Owner,
                ParentFolder = secondChild,
                Content = "Content"
            };
            var fileTwo = new TxtFile
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson 6",
                Owner = root.Owner,
                ParentFolder = secondChild,
                Content = "Content"
            };
            var fileThree = new TxtFile
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson 7",
                Owner = root.Owner,
                ParentFolder = secondChild,
                Content = "Content"
            };
            var fileFour = new TxtFile
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson 8",
                Owner = root.Owner,
                ParentFolder = secondChild,
                Content = "Content"
            };
            var AnotherFolder = new Folder
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson 9",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            var fileFive = new TxtFile
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Grandson 9 Child",
                Owner = root.Owner,
                ParentFolder = AnotherFolder,
                Content = "Content"
            };
            AnotherFolder.FolderChildren.Add(fileFive);
            secondChild.FolderChildren.Add(fileOne);
            secondChild.FolderChildren.Add(fileTwo);
            secondChild.FolderChildren.Add(fileThree);
            secondChild.FolderChildren.Add(fileFour);
            secondChild.FolderChildren.Add(AnotherFolder);

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
    }
}