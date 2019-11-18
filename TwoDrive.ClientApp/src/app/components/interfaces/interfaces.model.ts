export interface Writer {
  id: number;
  role: string;
  userName: string;
  password: string;
  friends: Writer[];
  claims: Claims[];
  isFriendsWithUserLoggedIn: Boolean;
}

export interface Element{
  folderChildren: Element[],
  id: number,
  name: string,
  parentFolderId: number,
  ownerId: number,
  creationDate: Date,
  dateModified: Date,
  isFolder: boolean
}

export interface ModificationReport{
  owner: string;
  amount: number;
}

export interface Claims{
  types: [],
  element: Element
}

export interface TopWritersReport{
  username: string;
  fileCount: number;
}

export interface ElementFlatNode {
  expandable: boolean;
  name: string;
  level: number;
}
