export interface Writer {
  id: number;
  role: string;
  userName: string;
  password: string;
  friends: Writer[];
  claims: Claims[];
  isFriendsWithUserLoggedIn: Boolean;
  hasClaimsForElement: Boolean
}

export interface Element {
  folderChildren: Element[],
  id: number,
  name: string,
  parentFolderId: number,
  ownerId: number,
  creationDate: Date,
  dateModified: Date,
  isFolder: boolean,
  ownerName: string,
  path: string,
  shouldRender: boolean,
  content: string
}

export interface ModificationReport {
  owner: string;
  amount: number;
}

export interface Claims {
  types: [],
  element: Element
}

export interface TopWritersReport {
  username: string;
  fileCount: number;
}

export interface ElementFlatNode {
  expandable: boolean;
  name: string;
  level: number;
  id: number,
  ownerId: number,
  hasChildrenLoaded: boolean;
  isChildFromLoggedInWriter: boolean;
  content: string,
  shouldRender: boolean
}

export interface AllFilesReport {
  id: number,
  name: string,
  creationDate: Date,
  dateModified: Date,
  ownerName: string
}

export interface importer {
  name: string
}


