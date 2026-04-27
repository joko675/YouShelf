export interface Book {
    id: number;
    title: string;
    author: string;
    rating: string | null;
    releaseYear: number | null;
    status: string;
    coverImagePathFixed: string;
}



export interface BookFormDto {
    id?: number;
    title: string;
    author: string;
    rating: string | null;
    releaseYear: number | null;
    status: string;
    coverImgPath: string;
}
