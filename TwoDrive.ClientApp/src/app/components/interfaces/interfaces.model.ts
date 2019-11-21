export interface Writer {
  id: number;
  role: string;
  userName: string;
  password: string;
  friends: Writer[];
  claims: Claims[];
  isFriendsWithUserLoggedIn: Boolean;
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
  content: string,
  shouldRender: Boolean,
  Type: string //Used for binding files in backend
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
  id: number
  hasChildrenLoaded: boolean;
  isChildFromLoggedInWriter: boolean;
}

export interface AllFilesReport {
  name: string,
  creationDate: Date,
  dateModified: Date,
  ownerName: string
}
