export interface Game {
    title: string,
    id: number,
    releaseDate: string,
    developer: string,
    publisher:string,
}

export interface Review {
    username: string,
    score: number,
    text: string,
    publicationDate: string,
}

export interface GameInfo {
    game: Game,
    avg_score: number,
    most_reviews: Review[],
    time_stat: string[],
}

export interface Time_record {
    username: string,
    time: string,
    time_record_type: Time_record_type,
}

export enum Time_record_type {
    Normal = 0,
    Full = 1,
}

export interface User {
    username: string,
    password: string,
}


export interface Context {
    id: number
    text: string
}

export interface Property {
    id: number
    name: string
}

export type ModelElement = Property;

export type Model = Property;
