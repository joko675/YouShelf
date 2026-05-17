import { path } from "@tauri-apps/api";
import { resourceDir } from "@tauri-apps/api/path";
import { copyFile, exists, mkdir, readTextFile, writeTextFile } from "@tauri-apps/plugin-fs";
import { Book, BookFormDto } from "../types/Book";
import { convertFileSrc } from "@tauri-apps/api/core";
import { useUserDataStore } from "../stores/UserDataStore";

const exeDirectory = await resourceDir();
const userDataLocation = await path.join(exeDirectory, "userData.json");
const coverImagesDir = await path.join(exeDirectory, "coverImages");

async function createUserData() {
    if (!await exists(userDataLocation)) await writeTextFile(userDataLocation, JSON.stringify([]));
}
async function createImagesFolder() {
    if (!await exists(coverImagesDir)) await mkdir(coverImagesDir);
}

export async function readUserData(): Promise<Book[]> {
    console.log("Reading json file data");
    await createUserData();

    const userData = await readTextFile(userDataLocation);
    const userDataJson: Book[] = JSON.parse(userData);
    
    return userDataJson;
}

export async function writeUserData() {
    console.log("Writing json file");
    const userDataStore = useUserDataStore();

    try {
        await writeTextFile(userDataLocation, JSON.stringify(userDataStore.userData));
    } catch (err) {
        console.log(err);
    }
}

export async function addNewBookService(dto: BookFormDto) {
    console.log("Adding Book...");
    const userDataStore = useUserDataStore();

    const newId = await getUniqueId();

    try {
        let imagePath;
        let imagePathFixed: string;
        if (dto.coverImgPath === "/blankCover.png"){
            imagePathFixed = dto.coverImgPath;
        } 
        else {
            imagePath = await saveCoverImg(newId, dto.coverImgPath);
            if (imagePath === "") return 0;
            imagePathFixed = convertFileSrc(imagePath);
        }
        
        const newBook: Book = {
            id: newId,
            title: dto.title,
            author: dto.author,
            releaseYear: dto.releaseYear,
            status: dto.status,
            rating: dto.rating,
            coverImagePathFixed: imagePathFixed
        }

        userDataStore.userData.push(newBook);
        
    } catch (err) {
        console.log("Error when creating new book: "+err);
    }
}

export async function editBookService(dto: BookFormDto) {
    if (!dto.id) return 0;
     
    console.log("Editing book");
    const userDataStore = useUserDataStore();

    const editedBookId = Number(dto.id);
    let imagePathFixed;

    try {
        //Check if image path is already fixed (if user didnt change it)
        if (dto.coverImgPath.startsWith("http://asset.localhost") || dto.coverImgPath ==="/blankCover.png") {
            imagePathFixed = dto.coverImgPath;
        } else {
            const imagePath = await saveCoverImg(editedBookId, dto.coverImgPath);
            if (imagePath != "") {
                imagePathFixed = convertFileSrc(imagePath);
            } else return 0;
        }

        const editedBookIndex = userDataStore.userData.findIndex((book) => Number(book.id) === Number(editedBookId));

        const editedBook: Book = {
            id: editedBookId,
            title: dto.title,
            author: dto.author,
            coverImagePathFixed: imagePathFixed,
            status: dto.status,
            rating: dto.rating,
            releaseYear: dto.releaseYear
        }
        
        userDataStore.userData[editedBookIndex] = editedBook;
    } catch (err){
        console.log("Error when editing book info: "+err)
    }
}

//Copy image from filePath to app cover images location. Returns saved file path if success, "" otherwise
async function saveCoverImg(bookId: number, filePath: string): Promise<string> {
    await createImagesFolder();

    const fileExtension = await path.extname(filePath);
    const fileName = bookId.toString()+"."+fileExtension;
    const coverImageLoc = await path.join(coverImagesDir, fileName);

    if (fileExtension.toLowerCase() === "jpg" || fileExtension.toLowerCase() === "png" || fileExtension.toLowerCase() === "webp" || fileExtension.toLowerCase() === "jpeg") {
        console.log("Copying source image");
        await copyFile(filePath, coverImageLoc);
        return coverImageLoc;
    } else {
        return "";
    }
}


async function getUniqueId(): Promise<number> {
    const userDataStore = useUserDataStore();
    let maxId: number = 0;

    userDataStore.userData.forEach(book => {
        if (maxId <= book.id) maxId = book.id+1;
    });

    return maxId;
}