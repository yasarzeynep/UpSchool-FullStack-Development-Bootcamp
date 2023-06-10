import React, { useState } from 'react';
import './App.css';

function App() {
    const [todos, setTodos] = useState<string[]>([]);
    const [inputValue, setInputValue] = useState('');

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setInputValue(e.target.value);
    };

    const handleAddTodo = () => {
        if (inputValue.trim() !== '') {
            setTodos([...todos, inputValue]);
            setInputValue('');
        }
    };

    const handleToggleTodo = (index: number) => {
        const updatedTodos = [...todos];
        updatedTodos[index] = updatedTodos[index].startsWith('✅ ')
            ? updatedTodos[index].substring(2)
            : `✅ ${updatedTodos[index]}`;
        setTodos(updatedTodos);
    };

    const handleDeleteTodo = (index: number) => {
        const updatedTodos = todos.filter((_, i) => i !== index);
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
                <ul className="todos-list">
                    {todos.map((todo, index) => (
                        <li
                            key={index}
                            className={`todo-item ${todo.startsWith('✅ ') ? 'completed' : ''}`}
                            onClick={() => handleToggleTodo(index)}
                        >
                            {todo}
                            <span className="delete-icon" onClick={() => handleDeleteTodo(index)}>
                🗑️
              </span>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
}

export default App;