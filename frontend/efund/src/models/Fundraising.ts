import Jar from "./Jar";

export default interface Fundraising{
    id: string;
    title: string;
    description: string;
    avatarUrl: string;
    userId: string;
    monobankJarId: string;
    isClosed: boolean;
    tags: string[];
    monobankJar: Jar;
}