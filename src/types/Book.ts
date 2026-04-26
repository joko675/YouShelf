export interface Book {
    id: number;
    title: string;
    author: string;
    rating?: string;
    releaseDate: string;
    status: string;
    coverImagePathFixed: string;
}



export interface BookFormDto {
    id?: number;
    title: string;
    author: string;
    rating?: string;
    releaseDate: string;
    status: string;
    coverImgPath: string;
}
