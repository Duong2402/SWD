export interface PagedResult<T> {
    items: T[]; // Danh sách các mục (ở đây là [])
    totalCount: number; // Tổng số mục
    pageSize: number; // Số mục trên mỗi trang
    pageNumber: number; // Trang hiện tại
    totalPages: number; // Tổng số trang
    hasPreviousPage: boolean; // Có trang trước không
    hasNextPage: boolean; // Có trang tiếp theo không
}