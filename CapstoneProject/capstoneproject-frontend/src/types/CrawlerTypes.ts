export type SeleniumLogDto ={
    message:string;
    sentOn:Date;
};

export type NotificationDto = {
    id:number | null;
    userId:string;
    message: string;
    sentOn:Date;
};