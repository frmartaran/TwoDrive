using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.DataAccess;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

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
            var folderValidator = new FolderValidator(folderRepository, fileRepository);


            folderRepository.Insert(root);
            folderRepository.Insert(parentFolderOrigin);
            folderRepository.Insert(parentFolderDestination);
            folderRepository.Save();
            fileRepository.Insert(fileToMove);
            fileRepository.Save();

            var elementLogicDependencies = new ElementLogicDependencies
            {
                FileRepository = fileRepository,
                FolderRepository = folderRepository,
                ElementValidator = folderValidator
            };

            var elementLogic = new ElementLogic(elementLogicDependencies);
            elementLogic.MoveElement(fileToMove, parentFolderDestination);

            var parentFolderDestinationChild = parentFolderDestination.FolderChildren.FirstOrDefault();
            Assert.AreEqual(4, parentFolderDestinationChild.Id);
        }
    }
}