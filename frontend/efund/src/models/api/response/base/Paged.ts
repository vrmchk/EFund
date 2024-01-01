export default interface Paged<T> {
    items: T[];
    pageNumber: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    hasPrevious: boolean;
    hasNext: boolean;
}