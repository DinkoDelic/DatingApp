import { Photo } from './photo';

export interface User {
    id: number;
    username: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: any;
    photoUrl: string;
    city: string;
    country: string;
    // Optional variables are marked with ? (Elvis operator), always have to come after required var.
    interests?: string;
    introduction?: string;
    lookingFor?: string;
    // Array of photos from Photo interface
    photos?: Photo[];
}
