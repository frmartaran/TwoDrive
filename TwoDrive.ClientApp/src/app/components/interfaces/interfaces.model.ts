export interface Writer {
    id: number;
    role: string;
    userName: string;
    friends: Writer[];
    claims: [];
    isFriendsWithUserLoggedIn: Boolean;
  }