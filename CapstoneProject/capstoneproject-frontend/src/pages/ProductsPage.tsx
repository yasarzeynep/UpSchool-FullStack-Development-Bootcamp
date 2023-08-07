import React, { useEffect, useState } from 'react';
import axios from 'axios';
import ExcelJS from 'exceljs';

const Products = () => {
    const [products, setProducts] = useState([]);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const backendUrl = import.meta.env.VITE_API_URL;
                const response = await axios.get(`${backendUrl}/api/Products/GetAll`);

                if (response.status === 200) {
                    setProducts(response.data);
                }
            } catch (error) {
                console.error('Error fetching products:', error);
            }
        };
        void fetchProducts();
    }, []);

    const handleDownloadExcel = async () => {
        const selectedProducts = products;

        const workbook = new ExcelJS.Workbook();
        const worksheet = workbook.addWorksheet('All Products');

        worksheet.columns = [
            { header: 'Product ID', key: 'id', width: 40 },
            { header: 'Order ID', key: 'orderId', width: 40 },
            { header: 'Name', key: 'name', width: 40 },
            { header: 'Is On Sale', key: 'isOnSale', width: 15 },
            { header: 'Price', key: 'price', width: 15 },
            { header: 'Sale Price', key: 'salePrice', width: 15 },
            { header: 'Image Path', key: 'picture', width: 40 },
        ];

        selectedProducts.forEach((product) => {
            worksheet.addRow({
                id: product.id,
                orderId: product.orderId,
                name: product.name,
                isOnSale: product.isOnSale ? 'Yes' : 'No',
                price: product.price,
                salePrice: product.salePrice || '-',
                picture: product.picture,
            });
        });

        worksheet.eachRow((row, rowNumber) => {
            row.eachCell((cell) => {
                cell.font = { color: { argb: '00000000' } };
                cell.fill = {
                    type: 'pattern',
                    pattern: 'solid',
                    fgColor: { argb: '033b8a' },
                };
                cell.alignment = { horizontal: 'center', vertical: 'center' };
            });
        });

        worksheet.getRow(1).eachCell((cell) => {
            cell.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: '99badd' },
            };
            cell.font = { color: { argb: '1b1b1b' } };
            cell.alignment = { horizontal: 'center', vertical: 'center' };
        });

        worksheet.eachRow((row, rowNumber) => {
            if (rowNumber > 1) {
                const fillColor = rowNumber % 2 === 0 ? 'FFFFFF' : 'ccdcec';
                row.eachCell((cell) => {
                    cell.fill = {
                        type: 'pattern',
                        pattern: 'solid',
                        fgColor: { argb: fillColor },
                    };
                });
            }
        });

        worksheet.eachRow((row, rowNumber) => {
            if (rowNumber !== 1) {
                const isOnSaleCell = row.getCell('isOnSale');
                if (isOnSaleCell.value === 'Yes') {
                    isOnSaleCell.fill = {
                        type: 'pattern',
                        pattern: 'solid',
                        fgColor: { argb: '03c03c' },
                    };
                } else {
                    isOnSaleCell.fill = {
                        type: 'pattern',
                        pattern: 'solid',
                        fgColor: { argb: 'ff0000' },
                    };
                }
            }
        });

        worksheet.properties.rowHeight = -1;

        ['A', 'B', 'C'].forEach((columnKey) => {
            const column = worksheet.getColumn(columnKey);
            let maxContentLength = column.header.length;
            selectedProducts.forEach((product) => {
                const cellValue = product[columnKey.toLowerCase()];
                if (cellValue && cellValue.length > maxContentLength) {
                    maxContentLength = cellValue.length;
                }
            });
            column.width = Math.min(Math.max(maxContentLength + 40, 40), 40);
        });

        worksheet.autoFilter = 'A1:G1';

        workbook.xlsx.writeBuffer().then(async (buffer) => {
            const blob = new Blob([buffer], {
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
            });
            const url = URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'products.xlsx';
            a.click();
            URL.revokeObjectURL(url);
        });
    };

    return (
        <div className="flex flex-col items-center justify-center bg-white dark:bg-gray-800">
            <div className="p-2 bg-white text-white border border-gray-200 rounded-lg shadow dark:bg-gray-800 dark:border-gray-700" style={{ width: '100vh' }}>
                <div className="container mx-auto px-4 text-white py-8">
                    <div className="mx-auto justify-items-center text-white text-center">
                        <button className="bg-blue-500 text-white font-bold py-2 px-4 rounded mb-4 mr-3" onClick={handleDownloadExcel}>
                            Create Excel File and Send
                        </button>
                    </div>
                    <div className="overflow-x-auto">
                        <table className="table-auto w-full">
                            <thead>
                            <tr>
                                <th className="px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">Image</th>
                                <th className="px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">Order ID</th>
                                <th className="px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">Name</th>
                                <th className="px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">Is On Sale</th>
                                <th className="px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">Price</th>
                                <th className="px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">Sale Price</th>
                                <th className="px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">Is Deleted</th>
                            </tr>
                            </thead>
                            <tbody>
                            {products.map((product) => (
                                <tr key={product.id}>
                                    <td className="border px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">
                                        <img src={product.picture} alt={product.name} className="w-24 h-24 object-cover" />
                                    </td>
                                    <td className="border px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">{product.orderId}</td>
                                    <td className="border px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">{product.name}</td>
                                    <td className="border px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">
                                        {product.isOnSale ? 'Yes' : 'No'}
                                    </td>
                                    <td className="border px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">{product.price}</td>
                                    <td className="border px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">
                                        {product.salePrice || '-'}
                                    </td>
                                    <td className="border px-4 py-2 text-white font-bold py-2 px-4 rounded mb-4">
                                        {product.isDeleted ? 'Yes' : 'No'}
                                    </td>
                                </tr>
                            ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Products;
