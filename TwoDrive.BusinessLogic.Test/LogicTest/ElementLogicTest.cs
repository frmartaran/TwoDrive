using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.DataAccess;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.BusinessLogic.Logic;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class ElementLogicTest
    {
        private TxtFile fileToMove;

        private Folder root;

        private Folder parentFolderOrigin;

        private Folder parentFolderDestination;

        [TestInitialize]
        public void SetUp()
        {
            var writer = new Writer();
            root = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChildren = new List<Element>(),
                Owner = writer
            };
            parentFolderOrigin = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "parent folder origin",
                Owner = writer,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            parentFolderDestination = new Folder
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "parent folder destination",
                Owner = writer,
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            fileToMove = new TxtFile
            {
                Id = 4,
                Content = "fileToMove",
                Name = "fileToMove",
                CreationDate = DateTime.Now,
                DateModified = DateTime.Now,
                ParentFolder = parentFolderOrigin,
                Owner = writer
            };
        }

        [TestMethod]
        public void MoveFileFromOneFolderToAnother()
        {
            var context = ContextFactory.GetMemoryContext("Move File From One Folder To Another");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var elementRepository = new Repository<Element>(context);
            var folderValidator = new FolderValidator(folderRepository);

            folderRepository.Insert(root);
            folderRepository.Insert(parentFolderOrigin);
            folderRepository.Insert(parentFolderDestination);
            folderRepository.Save();
            elementRepository.Insert(fileToMove);
            elementRepository.Save();

            var moveElementDependencies = new MoveElementDependencies
            {
                ElementRepository = elementRepository,
                ElementValidator = folderValidator
            };
            var folderLogic = new FolderLogic(folderRepository, fileRepository);
            folderLogic.MoveElement(fileToMove, parentFolderDestination, moveElementDependencies);

            var parentFolderDestinationChild = parentFolderDestination.FolderChildren.FirstOrDefault();
            Assert.AreEqual(4, parentFolderDestinationChild.Id);
        }

        [TestMethod]
        public void MoveFileInComplexStructure()
        {
            var context = ContextFactory.GetMemoryContext("Move File In Complex Structure");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var elementRepository = new Repository<Element>(context);
            var folderValidator = new FolderValidator(folderRepository);

            var dependecies = new ElementLogicDependencies
            {
                ElementValidator = new FolderValidator(folderRepository),
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
                ParentFolder = child,
                FolderChildren = new List<Element>()
            };
            var SecondGrandson = new Folder
            {
                Id = 4,
                Name = "Grandson 2",
                Owner = root.Owner,
                ParentFolder = child,
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

            var moveElementDependencies = new MoveElementDependencies
            {
                ElementRepository = elementRepository,
                ElementValidator = folderValidator
            };
            var folderLogic = new FolderLogic(folderRepository, fileRepository);
            folderLogic.MoveElement(fileThree, child, moveElementDependencies);

            var isFileThreeInNewDestination = child.FolderChildren.Where(c => c.Id == 12)
                .FirstOrDefault() != null;

            Assert.IsTrue(isFileThreeInNewDestination);
        }
    }
}