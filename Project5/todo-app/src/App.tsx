import React, { useState } from 'react';
import './App.css';

interface TodoItem {
    id: string;
    text: string;
    date: Date;
}

function App() {
    const [todos, setTodos] = useState<TodoItem[]>([]);
    const [inputValue, setInputValue] = useState('');

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setInputValue(e.target.value);
    };

    const handleAddTodo = () => {
        if (inputValue.trim() !== '') {
            const newTodo: TodoItem = {
                id: String(Date.now()),
                text: inputValue.substring(0, 250),
                date: new Date(),
            };
            setTodos([...todos, newTodo]);
            setInputValue('');
        }
    };

    const handleDeleteTodo = (id: string) => {
        const updatedTodos = todos.filter((todo) => todo.id !== id);
        setTodos(updatedTodos);
    };

    return (
        <div className="container">
            <div className="Todos">
                <h1 className="todos-title">Todos</h1>
                <div className="todos-input">
                    <input
                        type="text"
                        value={inputValue}
                        onChange={handleInputChange}
                        placeholder="Add a todo..."
                    />
                    <button onClick={handleAddTodo}>Add</button>
                </div>
                <div className="todos-list">
                    <div className="todo-header">
                        <span className="todo-header-text">Todo</span>
                        <span className="todo-header-text">Date</span>
                    </div>
                    {todos.map((todo) => (
                        <div key={todo.id} className="todo-item">
                            <span className="todo-text">{todo.text}</span>
                            <span className="todo-date">{todo.date.toLocaleString()}</span>
                            <span className="delete-icon" onClick={() => handleDeleteTodo(todo.id)}>
                🗑️
              </span>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default App;