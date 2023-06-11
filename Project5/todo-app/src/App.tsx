// App.tsx
import React, { useState } from 'react';
import './App.css';

interface TodoItem {
    id: string;
    text: string;
    date: Date;
    isCompleted: boolean;
}

function App() {
    const [todos, setTodos] = useState<TodoItem[]>([]);
    const [inputValue, setInputValue] = useState('');
    const [sortByDate, setSortByDate] = useState<'ascending' | 'descending'>('ascending');

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setInputValue(e.target.value);
    };

    const handleAddTodo = () => {
        if (inputValue.trim() !== '') {
            const newTodo: TodoItem = {
                id: String(Date.now()),
                text: inputValue.substring(0, 250),
                date: new Date(),
                isCompleted: false,
            };
            setTodos([...todos, newTodo]);
            setInputValue('');
        }
    };

    const handleDeleteTodo = (id: string) => {
        const updatedTodos = todos.filter((todo) => todo.id !== id);
        setTodos(updatedTodos);
    };

    const handleToggleComplete = (id: string) => {
        const updatedTodos = todos.map((todo) => {
            if (todo.id === id) {
                return { ...todo, isCompleted: !todo.isCompleted };
            }
            return todo;
        });
        setTodos(updatedTodos);
    };

    const handleSortByDate = () => {
        const sortedTodos = [...todos].sort((a, b) => {
            if (sortByDate === 'ascending') {
                return a.date.getTime() - b.date.getTime();
            } else {
                return b.date.getTime() - a.date.getTime();
            }
        });
        setTodos(sortedTodos);
        setSortByDate(sortByDate === 'ascending' ? 'descending' : 'ascending');
    };

    return (
        <div className="container">
            <div className="Todos">
                <h1 className="todos-title">Todos</h1>
                <p className="quote">"Take it easy but take it."</p>
                <div className="todos-input">
                    <input
                        type="text"
                        value={inputValue}
                        onChange={handleInputChange}
                        placeholder="Add a todo..."
                    />
                    <button className="add-button" onClick={handleAddTodo}>
                        Add
                    </button>
                    <button
                        className={`sort-button ${sortByDate === 'ascending' ? 'active' : ''}`}
                        onClick={handleSortByDate}
                    >
                        Sort by Date {sortByDate === 'ascending' ? '' : ''}
                    </button>
                </div>
                <table className="todos-table">
                    <thead>
                    <tr>
                        <th>Status</th>
                        <th>Task</th>
                        <th>Date and Time</th>
                        <th></th>
                    </tr>
                    </thead>
                    <tbody>
                    {todos.map((todo) => (
                        <tr key={todo.id}>
                            <td>
                                <input
                                    type="checkbox"
                                    checked={todo.isCompleted}
                                    onChange={() => handleToggleComplete(todo.id)}
                                />
                            </td>
                            <td
                                className={todo.isCompleted ? 'completed' : ''}
                                onClick={() => handleToggleComplete(todo.id)}
                            >
                                {todo.text}
                            </td>
                            <td>{todo.date.toLocaleString()}</td>
                            <td>
                  <span
                      className="delete-icon"
                      onClick={() => handleDeleteTodo(todo.id)}
                      title="Delete"
                  >
                    🗑️
                  </span>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>

            </div>
        </div>
    );
}

export default App;
