using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.DataAccess;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class ElementValidatorTest
    {
        private Writer owner;

        [TestInitialize]
        public void SetUp()
        {
            owner = new Writer
            {
                Id = 1,
                UserName = "Owner"
            };
        }

        [TestMethod]
        public void ValidRootFolder()
        {
            var context = ContextFactory.GetMemoryContext("Valid Root Folder");
            var folderRepository = new FolderRepository(context);
            var folder = new Folder
            {
                Name = "Root",
                Owner = owner,
                ParentFolder = null,
                FolderChildren = new List<Element>()
            };

            var validator = new FolderValidator(folderRepository);
            var isValid = validator.IsValid(folder);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidFolder()
        {
            var context = ContextFactory.GetMemoryContext("Valid Folder");
            var folderRepository = new FolderRepository(context);
            var root = new Folder
            {
                Name = "Root",
                Owner = owner,
                ParentFolder = null,
                FolderChildren = new List<Element>()
            };

            var child = new Folder
            {
                Name = "Child",
                Owner = owner,
                ParentFolder = root
            };

            var validator = new FolderValidator(folderRepository);
            var isValid = validator.IsValid(child);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidFile()
        {
            var root = new Folder
            {
                Name = "Root",
                Owner = owner,
                ParentFolder = null,
                FolderChildren = new List<Element>()
            };

            var file = new TxtFile
            {
                Name = "A file",
                Owner = owner,
                ParentFolder = root,
                CreationDate = new DateTime(2019, 9, 15),
                DateModified = new DateTime(2019, 9, 16)
            };

            var validator = new FileValidator();
            var isValid = validator.IsValid(file);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void InvalidFolderWithoutName()
        {
            var context = ContextFactory.GetMemoryContext("Invalid Folder Without Name");
            var folderRepository = new FolderRepository(context);
            var folder = new Folder
            {
                Name = "",
                ParentFolder = null,
                Owner = owner
            };

            var validator = new FolderValidator(folderRepository);
            var isValid = validator.IsValid(folder);

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void InvalidFileWithoutName()
        {
            var root = new Folder
            {
                Name = "Root",
                ParentFolder = null,
                Owner = owner
            };

            var file = new TxtFile
            {
                Name = "",
                Owner = owner,
                ParentFolder = root,
                CreationDate = new DateTime(2019, 9, 15),
                DateModified = new DateTime(2019, 9, 6)
            };

            var validator = new FileValidator();
            var isValid = validator.IsValid(file);

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void InvalidFolderWithoutOwner()
        {
            var context = ContextFactory.GetMemoryContext("Invalid Folder Without Owner");
            var folderRepository = new FolderRepository(context);
            var folder = new Folder
            {
                Name = "Root",
                ParentFolder = null,
                Owner = null
            };

            var validator = new FolderValidator(folderRepository);
            var isValid = validator.IsValid(folder);

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void InvalidFileWithoutOwner()
        {
            var root = new Folder
            {
                Name = "Root",
                ParentFolder = null,
                Owner = owner
            };

            var file = new TxtFile
            {
                Name = "A file",
                Owner = null,
                ParentFolder = root,
                CreationDate = new DateTime(2019, 9, 15),
                DateModified = new DateTime(2019, 9, 6)
            };

            var validator = new FileValidator();
            var isValid = validator.IsValid(file);

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void InvalidFolderWithoutParent()
        {
            var context = ContextFactory.GetMemoryContext("Invalid Folder Without Parent");
            var folderRepository = new FolderRepository(context);
            var folder = new Folder
            {
                Name = "A folder",
                ParentFolder = null,
                Owner = owner
            };

            var validator = new FolderValidator(folderRepository);
            var isValid = validator.IsValid(folder);

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void InvalidFileWithoutParent()
        {
            var file = new TxtFile
            {
                Name = "A file",
                Owner = owner,
                ParentFolder = null,
                CreationDate = new DateTime(2019, 9, 15),
                DateModified = new DateTime(2019, 9, 6)
            };

            var validator = new FileValidator();
            var isValid = validator.IsValid(file);

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TwoFoldersAtSameLevelWithSameName()
        {
            var context = ContextFactory.GetMemoryContext("Two Folders At Same Level With Same Name");
            var folderRepository = new FolderRepository(context);
            var root = new Folder
            {
                Name = "Root",
                ParentFolder = null,
                Owner = owner
            };

            var firstChild = new Folder
            {
                Name = "First",
                ParentFolder = root,
                Owner = owner
            };

            var secondChild = new Folder
            {
                Name = "First",
                ParentFolder = root,
                Owner = owner
            };

            var children = new List<Element>();
            children.Add(firstChild);
            root.FolderChildren = children;

            var validator = new FolderValidator(folderRepository);
            var isValid = validator.IsValid(secondChild);

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TwoFilesAtSameLevelWithSameName()
        {
            var root = new Folder
            {
                Name = "Root",
                ParentFolder = null,
                Owner = owner
            };

            var firstChild = new TxtFile
            {
                Name = "A file",
                Owner = owner,
                ParentFolder = root,
                CreationDate = new DateTime(2019, 9, 15),
                DateModified = new DateTime(2019, 9, 6)
            };

            var secondChild = new TxtFile
            {
                Name = "A file",
                Owner = owner,
                ParentFolder = root,
                CreationDate = new DateTime(2019, 9, 15),
                DateModified = new DateTime(2019, 9, 6)
            };

            var children = new List<Element>();
            children.Add(firstChild);
            root.FolderChildren = children;

            var validator = new FileValidator();
            var isValid = validator.IsValid(secondChild);

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void InvalidFileDateModifiedBeforeCreationDate()
        {
            var root = new Folder
            {
                Name = "Root",
                ParentFolder = null,
                Owner = owner,
                FolderChildren = new List<Element>()
            };

            var file = new TxtFile
            {
                Name = "A file",
                Owner = owner,
                ParentFolder = root,
                CreationDate = new DateTime(2019, 9, 15),
                DateModified = new DateTime(2019, 9, 6)
            };

            var validator = new FileValidator();
            var isValid = validator.IsValid(file);

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ElementDestinationDoesntExist()
        {
            var context = ContextFactory.GetMemoryContext("Element destination doesnt exist");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var writer = new Writer();
            var root = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var destination = new Folder
            {
                Id = 2,
                Name = "Folder",
                ParentFolder = root,
                Owner = owner,
                FolderChildren = new List<Element>()
            };
            var elementToTransfer = new TxtFile
            {
                Id = 3,
                Content = "TestFile",
                Name = "FileName",
                CreationDate = DateTime.Now,
                DateModified = DateTime.Now,
                ParentFolder = root,
                Owner = writer
            };

            var validator = new FolderValidator(folderRepository);
            var isValid = validator.IsValidDestination(elementToTransfer, destination);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ElementDestinationIsNotMyRootsChild()
        {
            var context = ContextFactory.GetMemoryContext("Element Destination Is Not my root's child");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var ownerOfFolderToTransfer = new Writer
            {
                Id = 1
            };
            var ownerOfFolderDestination = new Writer
            {
                Id = 2
            };
            var ownerOfFolderToTransferRoot = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>(),
                Owner = ownerOfFolderToTransfer
            };
            var ownerOfFolderDestinationRoot = new Folder
            {
                Id = 4,
                Name = "Root",
                FolderChildren = new List<Element>(),
                Owner = ownerOfFolderDestination
            };
            var destination = new Folder
            {
                Id = 5,
                Name = "Folder",
                ParentFolder = ownerOfFolderDestinationRoot,
                Owner = ownerOfFolderDestination,
                FolderChildren = new List<Element>()
            };
            var elementToTransfer = new TxtFile
            {
                Id = 6,
                Content = "TestFile",
                Name = "FileName",
                CreationDate = DateTime.Now,
                DateModified = DateTime.Now,
                ParentFolder = ownerOfFolderToTransferRoot,
                Owner = ownerOfFolderToTransfer
            };

            folderRepository.Insert(ownerOfFolderToTransferRoot);
            folderRepository.Insert(ownerOfFolderDestinationRoot);
            folderRepository.Insert(destination);
            fileRepository.Insert(elementToTransfer);
            fileRepository.Save();
            folderRepository.Save();

            var validator = new FolderValidator(folderRepository);
            var isValid = validator.IsValidDestination(elementToTransfer, destination);
        }

        
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ElementDestinationIsChildOfElementToTransfer()
        {
            var context = ContextFactory.GetMemoryContext("Element Destination Is Child Of Owner Parent Folder");
            var folderRepository = new FolderRepository(context);
            var writer = new Writer();
            var root = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChildren = new List<Element>(),
                Owner = writer
            };
            var elementToTransfer = new Folder
            {
                Id = 3,
                Name = "Folder",
                ParentFolder = root,
                Owner = writer,
                FolderChildren = new List<Element>()
            };
            var destination = new Folder
            {
                Id = 2,
                Name = "Folder",
                ParentFolder = elementToTransfer,
                Owner = writer,
                FolderChildren = new List<Element>()
            };

            folderRepository.Insert(root);
            folderRepository.Insert(elementToTransfer);
            folderRepository.Insert(destination);
            folderRepository.Save();

            var validator = new FolderValidator(folderRepository);
            var isValid = validator.IsValidDestination(elementToTransfer, destination);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ElementToTransferAndDestinationAreEmpty()
        {
            var context = ContextFactory.GetMemoryContext("Element To Transfer And Destination Are Empty");
            var folderRepository = new FolderRepository(context);
            var validator = new FolderValidator(folderRepository);
            var isValid = validator.IsValidDestination(null, null);
        }
    }
}