export interface Writer {
  id: number;
  role: string;
  userName: string;
  password: string;
  friends: Writer[];
  claims: [];
  isFriendsWithUserLoggedIn: Boolean;
}

export interface ModificationReport{
  owner: string;
  amount: number;
}

export interface TopWritersReport{
  username: string;
  fileCount: number;
}